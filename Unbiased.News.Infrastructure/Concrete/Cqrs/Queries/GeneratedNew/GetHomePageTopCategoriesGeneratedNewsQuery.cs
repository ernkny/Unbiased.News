using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetHomePageTopCategoriesGeneratedNewsQuery:IRequest<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>;
}
