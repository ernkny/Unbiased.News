using MediatR;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve all generated news items with the specified language.
    /// </summary>
    /// <param name="language">The language code to filter news by.</param>
    public record GetAllGeneratedNewsQuery(string language) : IRequest<IEnumerable<Entities.GeneratedNew>>;
}
