namespace Planday.Schedule.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<string?> GetEmployeeEmailAsync(long employeeId);
    }
}
