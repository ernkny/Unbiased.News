using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNews
{
    /// <summary>
    /// Query to retrieve all generated news with images.
    /// </summary>
    public record GetAllGeneratedNewsWithImageQuery : IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
