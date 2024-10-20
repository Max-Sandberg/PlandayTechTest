using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class UpdateShiftQuery : BaseQuery, IUpdateShiftQuery
    {
        private const string Sql = @"
            UPDATE Shifts
            SET EmployeeId = @EmployeeId, Start = @Start, End = @End
            WHERE Id = @Id;";

        public UpdateShiftQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task QueryAsync(Shift shift)
        {
            if (shift.Id is null)
            {
                throw new InvalidOperationException($"Cannot update a shift that does not have an id.");
            }

            await using var sqlConnection = GetConnection();

            await sqlConnection.ExecuteAsync(Sql, new { shift.Id, shift.EmployeeId, Start = shift.Start.ToString(), End = shift.End.ToString() });
        }
    }
}

