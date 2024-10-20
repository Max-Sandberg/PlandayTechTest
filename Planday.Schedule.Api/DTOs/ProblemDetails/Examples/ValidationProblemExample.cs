using Swashbuckle.AspNetCore.Filters;

namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class ValidationProblemExample : IExamplesProvider<ValidationProblem>
    {
        public ValidationProblem GetExamples()
            => new ValidationProblem("The request was invalid.", "/service"); // Could extend this class per endpoint if we really want the instance property to be correct.
    }
}
