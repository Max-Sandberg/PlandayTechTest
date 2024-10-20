using Planday.Schedule.Entities;

namespace Planday.Schedule.Infrastructure.DTOs
{
    // Strings because SqlLite can't do DateTime.
    public record DbShift(long? Id, long? EmployeeId, string Start, string End)
    {
        public DbShift(Shift shift)
            : this(shift.Id, shift.EmployeeId, shift.Start.ToString(), shift.End.ToString()) { }

        public Shift ToEntity()
            => new Shift(Id, EmployeeId, DateTime.Parse(Start), DateTime.Parse(End));
    }
}
