using Planday.Schedule.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public class ShiftAndEmployeeResponseExample : IExamplesProvider<ShiftAndEmployeeResponse>
    {
        public ShiftAndEmployeeResponse GetExamples()
            => new ShiftAndEmployeeResponse(
                new Shift(1, 1, DateTime.Today.AddHours(12), DateTime.Today.AddHours(18)),
                new Employee(1, "John Smith")
            );
    }
}
