using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetBannerGeneratedNewsWithImageQuery:IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
