using Planday.Schedule.Services.ExternalAPIs.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace Planday.Schedule.Services.ExternalAPIs
{
    public class ExternalEmployeeApi : IExternalEmployeeApi
    {
        private readonly HttpClient _httpClient;

        public ExternalEmployeeApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmployeeResponse?> GetEmployeeByIdAsync(long employeeId)
        {
            var response = await _httpClient.GetAsync($"employee/{employeeId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EmployeeResponse>();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Bad request is used to indicate the Employee doesn't exist. In this case don't throw, just return null.
                return null;
            }

            // If the response wasn't successful, throw an exception.
            throw new HttpRequestException($"Error retrieving employee with ID {employeeId}: {response.ReasonPhrase}");
        }
    }
}
