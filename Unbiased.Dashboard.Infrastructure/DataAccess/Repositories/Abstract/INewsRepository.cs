using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsRepository
    {
        Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<int> GetAllGeneratedNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto);
        Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id);
        Task<bool> UpdateGeneratedNewsWithImageAsync(UpdateGeneratedNewsDto generatedNewsDto);
        Task<bool> InsertGeneratedNewsWithImageAsync(InsertNewsWithImageDto insertNewsWithImageDto);

    }
}
