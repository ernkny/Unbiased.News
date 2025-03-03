using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetBannerGeneratedNewsWithImageQuery(int categoryId,string language):IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
