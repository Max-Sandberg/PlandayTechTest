using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.DTOs;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetShiftsByEmployeeQuery : BaseQuery, IGetShiftsByEmployeeQuery
    {
        private const string Sql = @"
            SELECT Id, EmployeeId, Start, End
            FROM Shifts
            WHERE EmployeeId = @EmployeeId;";

        public GetShiftsByEmployeeQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task<IReadOnlyCollection<Shift>> QueryAsync(long employeeId)
        {
            await using var sqlConnection = GetConnection();

            var dbShifts = await sqlConnection.QueryAsync<DbShift>(Sql, new { EmployeeId = employeeId });

            return dbShifts.Select(dbs => dbs.ToEntity()).ToList();
        }
    }
}

