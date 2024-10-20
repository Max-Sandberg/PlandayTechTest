using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public class OpenShiftResponseExample : IExamplesProvider<ShiftResponse>
    {
        public ShiftResponse GetExamples()
            => new ShiftResponse(1, null, DateTime.Today.AddHours(12), DateTime.Today.AddHours(18));
    }
}
