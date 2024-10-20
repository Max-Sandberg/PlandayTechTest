namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class ValidationProblem
    {
        public string Type => $"ValidationProblem";

        public string Title => $"Validation Failed";

        public int Status => StatusCodes.Status400BadRequest;

        public string Detail { get; }

        public string Instance { get; }

        public ValidationProblem(string detail, string instance)
        {
            Detail = detail;
            Instance = instance;
        }
    }
}
