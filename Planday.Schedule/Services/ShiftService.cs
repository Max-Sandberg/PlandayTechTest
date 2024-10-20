using Planday.Schedule.Entities;
using Planday.Schedule.Queries;
using Planday.Schedule.Services.Interfaces;

namespace Planday.Schedule.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IAddShiftQuery _addShiftQuery;

        private readonly IGetAllShiftsQuery _getAllShiftsQuery;

        private readonly IGetShiftByIdQuery _getShiftByIdQuery;

        private readonly IGetEmployeeByIdQuery _getEmployeeByIdQuery;

        private readonly IGetShiftsByEmployeeQuery _getShiftsByEmployeeQuery;

        private readonly IUpdateShiftQuery _updateShiftQuery;

        public ShiftService(
            IAddShiftQuery addShiftQuery,
            IGetAllShiftsQuery getAllShiftsQuery,
            IGetShiftByIdQuery getShiftByIdQuery,
            IGetEmployeeByIdQuery getEmployeeByIdQuery,
            IGetShiftsByEmployeeQuery getOverlappingShiftsQuery,
            IUpdateShiftQuery updateShiftQuery)
        {
            _addShiftQuery = addShiftQuery;
            _getAllShiftsQuery = getAllShiftsQuery;
            _getShiftByIdQuery = getShiftByIdQuery;
            _getEmployeeByIdQuery = getEmployeeByIdQuery;
            _getShiftsByEmployeeQuery = getOverlappingShiftsQuery;
            _updateShiftQuery = updateShiftQuery;
        }

        public async Task<IReadOnlyCollection<Shift>> GetAllShiftsAsync()
            => await _getAllShiftsQuery.QueryAsync();

        public async Task<Shift?> GetShiftByIdAsync(long id)
            => await _getShiftByIdQuery.QueryAsync(id);

        public async Task<CreateOpenShiftResult> CreateOpenShiftAsync(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                return CreateOpenShiftResult.StartAfterEnd();
            }

            if (start.Date != end.Date)
            {
                return CreateOpenShiftResult.StartAndEndDifferentDays();
            }

            // Create a shift with just the start and end date, and save to the DB.
            var shift = new Shift(null, null, start, end);
            var shiftId = await _addShiftQuery.QueryAsync(shift);

            // Add the returned id to our shift object and return it.
            shift.Id = shiftId;

            return CreateOpenShiftResult.Success(shift);
        }

        public async Task<AssignEmployeeToShiftResult> AssignEmployeeToShiftAsync(long shiftId, long employeeId)
        {
            // Check the employee exists.
            var employee = await _getEmployeeByIdQuery.QueryAsync(employeeId);
            if (employee is null)
            {
                return AssignEmployeeToShiftResult.EmployeeDoesNotExist();
            }

            // Check the shift exists.
            var shift = await _getShiftByIdQuery.QueryAsync(shiftId);
            if (shift is null)
            {
                return AssignEmployeeToShiftResult.ShiftDoesNotExist();
            }

            // If the shift is already assigned to this employee, return Ok.
            if (shift.EmployeeId == employeeId)
            {
                return AssignEmployeeToShiftResult.Success(shift, employee);
            }

            // Check there aren't any overlaps with existing shifts.
            var overlappingShifts = (await _getShiftsByEmployeeQuery.QueryAsync(employeeId))
                .Where(s => s.OverlapsWith(shift));

            if (overlappingShifts.Any())
            {
                return AssignEmployeeToShiftResult.ShiftConflict(shift, employee, overlappingShifts);
            }

            // Assign the shift to the employee.
            shift.AssignToEmployee(employeeId);
            await _updateShiftQuery.QueryAsync(shift);

            return AssignEmployeeToShiftResult.Success(shift, employee);
        }
    }
}
