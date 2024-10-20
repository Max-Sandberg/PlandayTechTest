namespace Planday.Schedule.Entities
{
    public class Shift
    {
        public Shift(long? id, long? employeeId, DateTime start, DateTime end)
        {
            Id = id;
            EmployeeId = employeeId;
            Start = start;
            End = end;
        }

        public long? Id { get; internal set; } // Id is nullable as shifts may not be saved to the DB.

        public long? EmployeeId { get; internal set; }

        public DateTime Start { get; internal set; }

        public DateTime End { get; internal set; }

        public bool OverlapsWith(Shift other)
            => other.Start < this.End && other.End > this.Start;

        public void AssignToEmployee(long employeeId)
            => this.EmployeeId = employeeId;
    }
}

