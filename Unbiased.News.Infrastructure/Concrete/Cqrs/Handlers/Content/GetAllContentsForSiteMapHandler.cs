using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    ///  Handler for retrieving all content entries for the sitemap in a specific language.
    /// </summary>
    public class GetAllContentsForSiteMapHandler : IRequestHandler<GetAllContentsForSiteMapQuery, IEnumerable<SitemapContentModel>>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        ///  Initializes a new instance of the GetAllContentsForSiteMapHandler class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public GetAllContentsForSiteMapHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the retrieval of all content entries for the sitemap in a specific language.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SitemapContentModel>> Handle(GetAllContentsForSiteMapQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetAllContentsForSiteMap(request.language);
        }
    }
}
