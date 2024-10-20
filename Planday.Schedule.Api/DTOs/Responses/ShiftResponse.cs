using Planday.Schedule.Entities;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public record ShiftResponse(long? Id, long? EmployeeId, DateTime Start, DateTime End)
    {
        public ShiftResponse(Shift shift)
            : this(shift.Id, shift.EmployeeId, shift.Start, shift.End) { }
    }
}
