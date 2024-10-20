using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class AddShiftQuery : BaseQuery, IAddShiftQuery
    {
        private const string Sql = @"
            INSERT INTO Shifts (EmployeeId, Start, End)
            VALUES (@EmployeeId, @Start, @End);
            SELECT last_insert_rowid();"; // Get the ID of the newly inserted row

        public AddShiftQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task<long> QueryAsync(Shift shift)
        {
            if (shift.Id is not null)
            {
                throw new InvalidOperationException($"Cannot add a shift that already has an id ({shift.Id}).");
            }

            await using var sqlConnection = GetConnection();

            return await sqlConnection.ExecuteScalarAsync<long>(Sql, new { shift.EmployeeId, Start = shift.Start.ToString(), End = shift.End.ToString() });
        }
    }
}

