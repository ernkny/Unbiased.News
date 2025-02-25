using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Query to retrieve all generated news with images.
    /// </summary>
    public record GetAllGeneratedNewsWithImageQuery(int categoryId, int pageNumber, string language) : IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
