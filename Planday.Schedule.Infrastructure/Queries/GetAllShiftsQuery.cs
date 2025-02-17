﻿using Dapper;
using Planday.Schedule.Entities;
using Planday.Schedule.Infrastructure.DTOs;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetAllShiftsQuery : BaseQuery, IGetAllShiftsQuery
    {
        private const string Sql = @"
            SELECT Id, EmployeeId, Start, End
            FROM Shifts;";

        public GetAllShiftsQuery(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public async Task<IReadOnlyCollection<Shift>> QueryAsync()
        {
            await using var sqlConnection = GetConnection();

            var dbShifts = await sqlConnection.QueryAsync<DbShift>(Sql);

            return dbShifts.Select(dbs => dbs.ToEntity()).ToList();
        }
    }
}

