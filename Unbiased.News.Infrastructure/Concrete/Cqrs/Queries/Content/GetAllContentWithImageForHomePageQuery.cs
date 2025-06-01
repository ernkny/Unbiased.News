using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    ///  Query to retrieve all content subheadings with associated images for the home page.
    /// </summary>
    /// <param name="language"></param>
    public record GetAllContentWithImageForHomePageQuery(string language) : IRequest<IEnumerable<ContentSubHeadingWithImageDto>>;
}
