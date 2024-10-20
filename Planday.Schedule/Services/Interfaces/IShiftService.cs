using Planday.Schedule.Entities;

namespace Planday.Schedule.Services.Interfaces
{
    public interface IShiftService
    {
        Task<IReadOnlyCollection<Shift>> GetAllShiftsAsync();

        Task<Shift?> GetShiftByIdAsync(long id);

        Task<CreateOpenShiftResult> CreateOpenShiftAsync(DateTime start, DateTime end);

        Task<AssignEmployeeToShiftResult> AssignEmployeeToShiftAsync(long shiftId, long employeeId);
    }
}
