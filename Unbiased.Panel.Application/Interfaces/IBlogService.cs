using Microsoft.AspNetCore.Http;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto);
        Task<BlogDto> GetBlogByIdWithImageAsync(string id);
        Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto);
        Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId, IFormFile file);
        Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto, IFormFile file);
        Task<bool> DeleteBlogAsync(string id);

    }
}
