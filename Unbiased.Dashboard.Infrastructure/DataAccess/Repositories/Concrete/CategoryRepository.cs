using Dapper;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    ///  Represents a repository for managing categories in the Unbiased Dashboard.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="eventAndActivityLog">The event and activity logging service.</param>
        public CategoryRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of categories.</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<Category>($"Exec UB_sp_GetAllcategoriesWithCount");
                    return result;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
