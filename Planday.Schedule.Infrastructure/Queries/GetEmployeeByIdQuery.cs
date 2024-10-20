using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.DTOs;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetEmployeeByIdQuery : BaseQuery, IGetEmployeeByIdQuery
    {
        private const string Sql = @"
            SELECT Id, Name
            FROM Employees
            WHERE Id = @Id;";

        public GetEmployeeByIdQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task<Employee?> QueryAsync(long id)
        {
            await using var sqlConnection = GetConnection();

            var dbEmployee = await sqlConnection.QueryFirstOrDefaultAsync<DbEmployee>(Sql, new { Id = id });

            return dbEmployee is null
                ? null
                : dbEmployee.ToEntity();
        }
    }
}

