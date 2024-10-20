using Microsoft.Data.Sqlite;
using Planday.Schedule.Infrastructure.Providers.Interfaces;

namespace Planday.Schedule.Infrastructure.Queries
{
    public abstract class BaseQuery
    {
        protected readonly IConnectionStringProvider _connectionStringProvider;

        protected BaseQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        protected SqliteConnection GetConnection()
            => new(_connectionStringProvider.GetConnectionString());
    }
}
