using Dapper;
using System.Data;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    public class ContactRepository: IContactRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContactRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }
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
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
