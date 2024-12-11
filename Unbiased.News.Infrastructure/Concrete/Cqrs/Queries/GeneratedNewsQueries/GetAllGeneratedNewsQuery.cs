using MediatR;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetAllGeneratedNewsQuery() : IRequest<IEnumerable<Entities.GeneratedNews>>;
}
