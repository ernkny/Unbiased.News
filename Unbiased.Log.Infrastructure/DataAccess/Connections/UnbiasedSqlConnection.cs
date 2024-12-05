using Microsoft.Data.SqlClient;
using System.Data;

namespace Unbiased.Log.Infrastructure.DataAccess.Connections
{
    /// <summary>
    /// Represents a connection to an Unbiased SQL database.
    /// </summary>
    public class UnbiasedSqlConnection
    {
        /// <summary>
        /// The connection string used to connect to the database.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnbiasedSqlConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public UnbiasedSqlConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new database connection using the stored connection string.
        /// </summary>
        /// <returns>A new <see cref="IDbConnection"/> instance.</returns>
        public IDbConnection CreateConnection()
            => new SqlConnection(connectionString); 
    }
}
