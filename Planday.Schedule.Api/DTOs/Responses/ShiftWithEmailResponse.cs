using Planday.Schedule.Entities;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public record ShiftWithEmailResponse(long? Id, long? EmployeeId, string? EmployeeEmail, DateTime Start, DateTime End)
    {
        public ShiftWithEmailResponse(Shift shift, string? email)
            : this(shift.Id, shift.EmployeeId, email, shift.Start, shift.End) { }
    }
}
