using Planday.Schedule.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class ShiftConflictProblemExample : IExamplesProvider<ShiftConflictProblem>
    {
        public ShiftConflictProblem GetExamples()
            => new ShiftConflictProblem(
                new Employee(1, "John Smith"),
                new Shift(1, null, DateTime.Today.AddHours(12), DateTime.Today.AddHours(18)),
                new[]
                {
                    new Shift(2, 1, DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                    new Shift(3, 1, DateTime.Today.AddHours(17), DateTime.Today.AddHours(20))
                },
                "shifts/assign?shiftId=1&employeeId=1"
            );
    }
}
