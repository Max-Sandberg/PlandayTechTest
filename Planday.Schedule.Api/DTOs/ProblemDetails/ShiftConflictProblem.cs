using Planday.Schedule.Api.DTOs.Responses;
using Planday.Schedule.Entities;

namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class ShiftConflictProblem
    {
        // Petty, but if you extend ProblemDetails, json deserialisation orders the base properties after the custom ones.
        // The swagger response examples read a bit nicer if the standard type/title etc come first, so just define them ourselves in the order we want.
        public string Type => "ShiftConflictProblem";

        public string Title => "Conflicting Shifts Detected";

        public int Status => StatusCodes.Status409Conflict;

        public string Detail => "This employee is already assigned to shifts in this time period. Assignment would result in conflicting shifts.";

        public string Instance { get; }

        public EmployeeResponse Employee { get; }

        public ShiftResponse Shift { get; }

        public IEnumerable<ShiftResponse> OverlappingShifts { get; }

        public ShiftConflictProblem(Employee employee, Shift shift, IEnumerable<Shift> overlappingShifts, string instance)
        {
            Employee = new EmployeeResponse(employee);
            Shift = new ShiftResponse(shift);
            OverlappingShifts = overlappingShifts.Select(s => new ShiftResponse(s));
            Instance = instance;
        }
    }
}
