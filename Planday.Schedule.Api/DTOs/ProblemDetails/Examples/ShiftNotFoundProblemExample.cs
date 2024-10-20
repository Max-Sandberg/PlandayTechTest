using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class ShiftNotFoundProblemExample : IExamplesProvider<NotFoundProblem>
    {
        public NotFoundProblem GetExamples()
            => new NotFoundProblem("Shift", 1, "/shifts/1");
    }
}
