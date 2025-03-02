using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface IBlogRepository
    {
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto, int pageNumber, int pageSize); 
        Task<BlogDto> GetBlogByIdWithImageAsync(string id);
        Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto);
        Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId);
        Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto);
        Task<bool> DeleteBlogByIdAsync(string id);
    }
}
