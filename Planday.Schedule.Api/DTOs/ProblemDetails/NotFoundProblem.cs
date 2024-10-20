namespace Planday.Schedule.Api.DTOs.ProblemDetails
{
    public class NotFoundProblem
    {
        public string Type => "NotFoundProblem";

        public string Title => $"{ItemName} Not Found";

        public int Status => StatusCodes.Status404NotFound;

        public string Detail => $"No {ItemName.ToLower()} found with ID {Id}.";

        public string Instance { get; }

        private string ItemName { get; }

        private long Id { get; }

        public NotFoundProblem(string itemName, long id, HttpContext httpContext)
            => new NotFoundProblem(itemName, id, httpContext.Request.Path.Value!);


        public NotFoundProblem(string itemName, long id, string instance)
        {
            ItemName = itemName;
            Id = id;
            Instance = instance;
        }
    }
}
