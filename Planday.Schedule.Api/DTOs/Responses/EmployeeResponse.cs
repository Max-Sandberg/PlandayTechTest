using Planday.Schedule.Entities;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public record EmployeeResponse(long? Id, string Name)
    {
        public EmployeeResponse(Employee employee)
            : this(employee.Id, employee.Name) { }
    }
}
