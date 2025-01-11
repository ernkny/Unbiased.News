using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.CategoriesQueris
{
    public record GetHomePageCategorieSliderWithCountQuery() : IRequest<IEnumerable<HomePageCategorieSliderWithCountDto>>;
}
