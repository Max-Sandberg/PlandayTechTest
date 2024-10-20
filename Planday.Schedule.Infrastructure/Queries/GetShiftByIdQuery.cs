using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.DTOs;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetShiftByIdQuery : BaseQuery, IGetShiftByIdQuery
    {
        private const string Sql = @"
            SELECT Id, EmployeeId, Start, End
            FROM Shifts
            WHERE Id = @Id;";

        public GetShiftByIdQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task<Shift?> QueryAsync(long id)
        {
            await using var sqlConnection = GetConnection();

            var dbShift = await sqlConnection.QueryFirstOrDefaultAsync<DbShift>(Sql, new { Id = id });

            return dbShift is null
                ? null
                : dbShift.ToEntity();
        }
    }
}

