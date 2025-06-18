using Dapper;
using System.Data;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    ///  Represents a repository for managing customer contact messages in the Unbiased Dashboard.
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="eventAndActivityLog">The event and activity logging service.</param>
        public ContactRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all customer messages with pagination support.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of customer contact messages.</returns>
        public async Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pageNumber", pageNumber);
                    parameters.Add("@pageSize", pageSize);
                    return await connection.QueryAsync<Contact>("UB_sp_GetAllCustomerMessages", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves a customer message by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer contact message.</returns>
        public async Task<Contact> GetCustomerMessagesByIdAsync(int id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
                    return await connection.QueryFirstAsync<Contact>("UB_sp_GetCustomerMessagesById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Deletes a customer message by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer message to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteCustomerMessagesByIdAsync(int id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
                    return await connection.QueryFirstAsync<bool>("UB_sp_DeleteCustomerMessagesById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}