using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News
{
    public record GetGeneratedNewsQuery(GetGeneratedNewsWithImagePathRequestDto GetGeneratedNewsWithImagePathRequestDto):IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
