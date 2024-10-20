using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.Requests.Examples
{
    public class CreateOpenShiftRequestExample : IExamplesProvider<CreateOpenShiftRequest>
    {
        public CreateOpenShiftRequest GetExamples()
            => new CreateOpenShiftRequest(DateTime.Today.AddDays(1).AddHours(12), DateTime.Today.AddDays(1).AddHours(15));
    }
}
