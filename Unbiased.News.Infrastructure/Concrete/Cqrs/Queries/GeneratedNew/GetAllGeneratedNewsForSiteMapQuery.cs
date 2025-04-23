using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetAllGeneratedNewsForSiteMapQuery(string language):IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
