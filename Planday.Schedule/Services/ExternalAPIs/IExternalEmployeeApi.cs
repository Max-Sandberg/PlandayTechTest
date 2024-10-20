using Planday.Schedule.Services.ExternalAPIs.DTOs;

namespace Planday.Schedule.Services.ExternalAPIs
{
    public interface IExternalEmployeeApi
    {
        Task<EmployeeResponse?> GetEmployeeByIdAsync(long employeeId);
    }
}
