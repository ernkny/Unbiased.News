using Dapper;
using System.Data;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    ///  Repository class for managing contact form submissions.
    /// </summary>
    public class ContactRepository: IContactRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContactRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Saves a contact form submission to the database.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<bool> SaveContact(Contact contact)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Subject", contact.Subject);
                    parameters.Add("@Message", contact.Message);
                    parameters.Add("@FullName", contact.FullName);
                    parameters.Add("@Email", contact.Email);
                    parameters.Add("@IsDeleted", false);
                    parameters.Add("@IsActive", true);
                    parameters.Add("@CreatedTime", DateTime.UtcNow);

                    await connection.ExecuteAsync("UB_sp_InsertContactForm", parameters, commandType: CommandType.StoredProcedure);
                    return true;
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
                }, _serviceProvider);
                throw;
            }
        }

    }
}
