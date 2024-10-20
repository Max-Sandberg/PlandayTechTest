using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public class ShiftResponseExample : IExamplesProvider<ShiftResponse>
    {
        public ShiftResponse GetExamples()
            => new ShiftResponse(1, 1, DateTime.Today.AddHours(12), DateTime.Today.AddHours(18));
    }
}
