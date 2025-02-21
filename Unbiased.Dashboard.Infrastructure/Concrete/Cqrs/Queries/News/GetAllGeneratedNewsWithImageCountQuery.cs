using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News
{
    public record GetAllGeneratedNewsWithImageCountQuery(GetGeneratedNewsWithImagePathRequestDto requestDto):IRequest<int>;
}
