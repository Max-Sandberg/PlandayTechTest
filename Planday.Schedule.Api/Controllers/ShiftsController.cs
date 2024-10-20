using Microsoft.AspNetCore.Mvc;
using Planday.Schedule.Api.DTOs.ProblemDetails;
using Planday.Schedule.Api.DTOs.Requests;
using Planday.Schedule.Api.DTOs.Requests.Examples;
using Planday.Schedule.Api.DTOs.Responses;
using Planday.Schedule.Entities;
using Planday.Schedule.Services;
using Planday.Schedule.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Planday.Schedule.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        private readonly IEmployeeService _employeeService;

        public ShiftsController(IShiftService shiftService, IEmployeeService employeeService)
        {
            _shiftService = shiftService;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Gets all saved shifts.
        /// </summary>
        /// <returns>All saved shifts.</returns>
        /// <response code="200">Shifts retrieved successfully.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShiftResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MultipleShiftResponsesExample))]
        public async Task<ActionResult<IEnumerable<ShiftResponse>>> GetAllShiftsAsync()
        {
            var shifts = await _shiftService.GetAllShiftsAsync();

            return Ok(shifts.Select(s => new ShiftResponse(s)));
        }

        /// <summary>
        /// Gets a shift by its ID.
        /// </summary>
        /// <param name="id">The ID of the shift.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="200">Shift retrieved successfully.</response>
        /// <response code="404">Shift was not found.</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftWithEmailResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ShiftWithEmailResponseExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundProblem))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ShiftNotFoundProblemExample))]
        public async Task<ActionResult<ShiftWithEmailResponse>> GetShiftByIdAsync([Required] long id)
        {
            Shift? shift = await _shiftService.GetShiftByIdAsync(id);

            if (shift is null)
            {
                return NotFound(new NotFoundProblem("Shift", id, HttpContext.Request.Path.Value!));
            }

            // If we have a shift, and it has an employee id, find the email for that shift.
            // Could think about including the email in all places we return shift/employees, but probably out of scope.
            string? email = null;
            if (shift.EmployeeId is not null)
            {
                email = await _employeeService.GetEmployeeEmailAsync(shift.EmployeeId.Value);
            }

            return Ok(new ShiftWithEmailResponse(shift, email));
        }

        /// <summary>
        /// Creates an open shift, i.e. a shift with no employee assigned.
        /// </summary>
        /// <param name="openShift">The open shift to create.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="200">Open shift successfully created.</response>
        /// <response code="400">A validation problem occurred.</response>
        [HttpPost("open")]
        [SwaggerRequestExample(typeof(CreateOpenShiftRequest), typeof(CreateOpenShiftRequestExample))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OpenShiftResponseExample))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblem))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationProblemExample))]
        public async Task<ActionResult> CreateOpenShift([FromBody, Required] CreateOpenShiftRequest openShift)
        {
            var result = await _shiftService.CreateOpenShiftAsync(openShift.Start, openShift.End);

            return result.Type switch
            {
                CreateOpenShiftResult.ResultType.Success
                    => Ok(new ShiftResponse(result.Shift!)),

                CreateOpenShiftResult.ResultType.StartAfterEnd
                    => BadRequest(new ValidationProblem("Start time must be before end time.", HttpContext.Request.Path.Value!)),

                CreateOpenShiftResult.ResultType.StartAndEndDifferentDays
                    => BadRequest(new ValidationProblem("Start time must be before end time.", HttpContext.Request.Path.Value!)),

                _ => throw new InvalidEnumArgumentException(nameof(result.Type), (int)result.Type, typeof(AssignEmployeeToShiftResult.ResultType))
            };
        }

        /// <summary>
        /// Assigns an employee to a shift.
        /// </summary>
        /// <param name="shiftId">The ID of the shift.</param>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="200">Employee successfully assigned to shift.</response>
        /// <response code="404">Shift or employee was not found.</response>
        /// <response code="409">Assignment would result in conflicting shifts.</response>
        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShiftAndEmployeeResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ShiftAndEmployeeResponseExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundProblem))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ShiftNotFoundProblemExample))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ShiftConflictProblem))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ShiftConflictProblemExample))]
        public async Task<ActionResult> AssignEmployeeToShiftAsync([Required] long shiftId, [Required] long employeeId)
        {
            var result = await _shiftService.AssignEmployeeToShiftAsync(shiftId, employeeId);

            return result.Type switch
            {
                AssignEmployeeToShiftResult.ResultType.Success
                    => Ok(new ShiftAndEmployeeResponse(result.Shift!, result.Employee!)),

                AssignEmployeeToShiftResult.ResultType.ShiftDoesNotExist
                    => NotFound(new NotFoundProblem("Shift", shiftId, HttpContext.Request.Path.Value!)),

                AssignEmployeeToShiftResult.ResultType.EmployeeDoesNotExist
                    => NotFound(new NotFoundProblem("Employee", employeeId, HttpContext.Request.Path.Value!)),

                AssignEmployeeToShiftResult.ResultType.ShiftConflict
                    => Conflict(new ShiftConflictProblem(result.Employee!, result.Shift!, result.OverlappingShifts!, HttpContext.Request.Path.Value!)),

                _ => throw new InvalidEnumArgumentException(nameof(result.Type), (int)result.Type, typeof(AssignEmployeeToShiftResult.ResultType))
            };
        }
    }
}

