using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class BlogRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public BlogRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }
    }
}
