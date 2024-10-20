using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Responses
{
    public class ShiftWithEmailResponseExample : IExamplesProvider<ShiftWithEmailResponse>
    {
        public ShiftWithEmailResponse GetExamples()
            => new ShiftWithEmailResponse(1, 1, "john@smith.com", DateTime.Today.AddHours(12), DateTime.Today.AddHours(18));
    }
}
