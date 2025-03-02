using Dapper;
using System.Data;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class NewsRepository:INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public NewsRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters(); 
                    parameters.Add("@pageNumber", requestDto.PageNumber);
                    parameters.Add("@pageSize", requestDto.PageSize);
                    parameters.Add("@Language", requestDto.Language);
                    parameters.Add("@IsApproved", requestDto.IsApproved);
                    parameters.Add("@categoryId", requestDto.CategoryId);
                    parameters.Add("@SearchData", requestDto.SearchData);
                    parameters.Add("@StartDate", requestDto.StartDate);
                    parameters.Add("@EndDate", requestDto.EndDate);
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllGeneratedNewsWithImagePathForDashboard", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Language", requestDto.Language);
                    parameters.Add("@IsApproved", requestDto.IsApproved);
                    parameters.Add("@categoryId", requestDto.CategoryId);
                    parameters.Add("@SearchData", requestDto.SearchData);
                    parameters.Add("@StartDate", requestDto.StartDate);
                    parameters.Add("@EndDate", requestDto.EndDate);
                    return await connection.QueryFirstAsync<int>("UB_sp_GetAllGeneratedNewsWithImagePathForDashboardCount", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteNewsByIdAsync(string id)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", id);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_DeleteNews", paramaters, commandType: CommandType.StoredProcedure)==1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateGeneratedNewsWithImageAsync(UpdateGeneratedNewsDto generatedNewsDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", generatedNewsDto.Id,DbType.String);
                parameters.Add("@Title", generatedNewsDto.Title,DbType.String);
                parameters.Add("@Detail", generatedNewsDto.Detail,DbType.String);
                parameters.Add("@CategoryId", generatedNewsDto.CategoryId,DbType.Int32);
                parameters.Add("@CreatedTime", generatedNewsDto.CreatedTime,DbType.DateTime);
                parameters.Add("@IsApproved", generatedNewsDto.IsApproved,DbType.Boolean);
                parameters.Add("@IsActive", generatedNewsDto.IsActive,DbType.Boolean);
                parameters.Add("@ImagePath", generatedNewsDto.ImagePath,DbType.String);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_UpdateGeneratedNews", parameters, commandType: CommandType.StoredProcedure)==1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertGeneratedNewsWithImageAsync(InsertNewsWithImageDto insertNewsWithImageDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", insertNewsWithImageDto.Title, DbType.String);
                parameters.Add("@Detail", insertNewsWithImageDto.Detail, DbType.String);
                parameters.Add("@CategoryId", insertNewsWithImageDto.CategoryId, DbType.Int32);
                parameters.Add("@Language", insertNewsWithImageDto.Language, DbType.String);
                parameters.Add("@IsActive", insertNewsWithImageDto.IsActive, DbType.Boolean);
                parameters.Add("@IsApproved", insertNewsWithImageDto.IsApproved, DbType.Boolean);
                parameters.Add("@Path", insertNewsWithImageDto.ImagePath, DbType.String);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_InsertIndividualNewsWithImage", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception)
            {
                throw; 
            }
        }

    }
}
