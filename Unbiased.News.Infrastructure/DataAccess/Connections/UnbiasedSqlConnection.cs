using Microsoft.Data.SqlClient;
using System.Data;

namespace Unbiased.News.Infrastructure.DataAccess.Connections
{
    public class UnbiasedSqlConnection
    {
        private readonly string connectionString;
        public UnbiasedSqlConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(connectionString);
    }
}