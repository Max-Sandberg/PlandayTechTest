namespace Planday.Schedule.Entities
{
    public class Employee
    {
        public Employee(long? id, string name)
        {
            Id = id;
            Name = name;
        }

        public long? Id { get; internal set; } // Id is nullable as employees may not be saved to the DB.

        public string Name { get; internal set; }
    }
}

