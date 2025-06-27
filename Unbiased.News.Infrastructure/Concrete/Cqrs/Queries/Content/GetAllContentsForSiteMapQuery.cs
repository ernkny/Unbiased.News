using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    ///  Query to retrieve all content entries for the sitemap in a specific language.
    /// </summary>
    /// <param name="language"></param>
    public record GetAllContentsForSiteMapQuery(string language) : IRequest<IEnumerable<SitemapContentModel>>;
}
