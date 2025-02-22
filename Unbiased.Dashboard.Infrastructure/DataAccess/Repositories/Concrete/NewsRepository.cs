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
    }
}
