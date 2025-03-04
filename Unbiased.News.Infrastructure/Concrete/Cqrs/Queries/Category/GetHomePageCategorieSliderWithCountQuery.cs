using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Category
{
    public record GetHomePageCategorieSliderWithCountQuery(string language) : IRequest<IEnumerable<HomePageCategorieSliderWithCountDto>>;
}
