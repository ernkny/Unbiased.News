using MediatR;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve all generated news.
    /// </summary>
    public record GetAllGeneratedNewsQuery(string language) : IRequest<IEnumerable<Entities.GeneratedNew>>;
}
