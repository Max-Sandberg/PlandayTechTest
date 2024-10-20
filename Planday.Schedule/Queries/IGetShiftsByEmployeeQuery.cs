using Planday.Schedule.Entities;

namespace Planday.Schedule.Queries
{
    public interface IGetShiftsByEmployeeQuery
    {
        Task<IReadOnlyCollection<Shift>> QueryAsync(long employeeId);
    }
}

