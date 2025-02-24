using Dapper;
using System.Data;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class BlogRepository: IBlogRepository
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

        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto, int pageNumber, int pageSize)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@PageNumber", pageNumber);
                paramaters.Add("@PageSize", pageSize);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);
                paramaters.Add("@SearchData", blogRequestDto.SearchData);
                paramaters.Add("@StartDate", blogRequestDto.StartDate);
                paramaters.Add("@EndDate", blogRequestDto.EndDate);

                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<BlogDto>("UB_sp_GetAllBlogsWithImageForDashboard", paramaters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);
                paramaters.Add("@SearchData", blogRequestDto.SearchData);
                paramaters.Add("@StartDate", blogRequestDto.StartDate);
                paramaters.Add("@EndDate", blogRequestDto.EndDate);

                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<int>("UB_sp_GetAllBlogsWithImageForDashboardCount", paramaters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<BlogDto> GetBlogByIdWithImageAsync(string id)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", id);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<BlogDto>("UB_sp_GetBlogById", paramaters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto,int UserId)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Title", blogRequestDto.Title);
                paramaters.Add("@Detail", blogRequestDto.Detail);
                paramaters.Add("@UserId", UserId);
                paramaters.Add("@Path", blogRequestDto.Path);
                paramaters.Add("@CreatedTime", DateTime.UtcNow);
                paramaters.Add("@IsActive", blogRequestDto.IsActive);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_InsertBlog", paramaters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", blogRequestDto.Id);
                paramaters.Add("@Title", blogRequestDto.Title);
                paramaters.Add("@Detail", blogRequestDto.Detail);
                paramaters.Add("@Path", blogRequestDto.Path);
                paramaters.Add("@CreatedTime", DateTime.UtcNow);
                paramaters.Add("@IsActive", blogRequestDto.IsActive);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_UpdateGeneratedBlog", paramaters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
