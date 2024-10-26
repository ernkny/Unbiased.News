using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        public CategoriesRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using (var connection= _connection.CreateConnection())
            {
                var result=await connection.QueryAsync<Category>("Select * from UB_Categories");
                return result;
            }
        }
    }
}
