using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public class MultipleShiftResponsesExample : IExamplesProvider<IEnumerable<ShiftResponse>>
    {
        public IEnumerable<ShiftResponse> GetExamples()
            => new[] {
                new ShiftResponse(1, 1, DateTime.Today.AddHours(12), DateTime.Today.AddHours(15)),
                new ShiftResponse(2, 2, DateTime.Today.AddHours(15), DateTime.Today.AddHours(18)),
            };
    }
}
