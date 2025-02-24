using Microsoft.AspNetCore.Http;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);
        Task<bool> UpdateGeneratedNewsWithImageAsync(IFormFile file, UpdateGeneratedNewsDto generateNewsWithImageDto);
        Task<bool> InsertNewsWithImageAsync(IFormFile file, InsertNewsWithImageDto insertNewsWithImageDto);
    }
}
