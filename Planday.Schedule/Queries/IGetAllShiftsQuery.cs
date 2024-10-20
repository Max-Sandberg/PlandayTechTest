using Planday.Schedule.Entities;

namespace Planday.Schedule.Queries
{
    public interface IGetAllShiftsQuery
    {
        Task<IReadOnlyCollection<Shift>> QueryAsync();
    }
}

