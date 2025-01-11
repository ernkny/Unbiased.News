using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetHomePageTopCategoriesGeneratedNewsQuery:IRequest<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>;
}
