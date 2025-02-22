using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGenerateNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<int> GetAllGenerateNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);
    }
}
