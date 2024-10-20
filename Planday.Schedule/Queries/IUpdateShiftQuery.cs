using Planday.Schedule.Entities;

namespace Planday.Schedule.Queries
{
    public interface IUpdateShiftQuery
    {
        Task QueryAsync(Shift shift);
    }
}

