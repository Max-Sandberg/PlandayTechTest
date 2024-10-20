using Planday.Schedule.Entities;

namespace Planday.Schedule.Infrastructure.DTOs
{
    public record DbEmployee(long? Id, string Name)
    {

        public DbEmployee(Employee employee)
            : this(employee.Id, employee.Name) { }

        public Employee ToEntity()
            => new Employee(Id, Name);
    }
}
