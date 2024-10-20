using Planday.Schedule.Services.ExternalAPIs;
using Planday.Schedule.Services.Interfaces;

namespace Planday.Schedule.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IExternalEmployeeApi _externalEmployeeApi;

        public EmployeeService(IExternalEmployeeApi externalEmployeeService)
        {
            _externalEmployeeApi = externalEmployeeService;
        }

        public async Task<string?> GetEmployeeEmailAsync(long employeeId)
        {
            var employeeResponse = await _externalEmployeeApi.GetEmployeeByIdAsync(employeeId);

            return employeeResponse?.Email;
        }
    }
}
