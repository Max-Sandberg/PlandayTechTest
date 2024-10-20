using Planday.Schedule.Entities;

namespace Planday.Schedule.Queries
{
    public interface IAddShiftQuery
    {
        Task<long> QueryAsync(Shift shift);
    }
}

