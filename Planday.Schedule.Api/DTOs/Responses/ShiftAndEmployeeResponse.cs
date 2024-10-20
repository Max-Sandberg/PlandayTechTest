using Planday.Schedule.Entities;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public record ShiftAndEmployeeResponse(ShiftResponse Shift, EmployeeResponse Employee)
    {
        public ShiftAndEmployeeResponse(Shift shift, Employee employee)
            : this(new ShiftResponse(shift), new EmployeeResponse(employee)) { }
    }
}
