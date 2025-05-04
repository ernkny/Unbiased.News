using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    /// Handler class for processing GetContentDetailQuery requests.
    /// Retrieves detailed content information based on a unique URL.
    /// </summary>
    public class GetContentDetailHandler:IRequestHandler<GetContentDetailQuery, GeneratedContentDto>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the GetContentDetailHandler class.
        /// </summary>
        /// <param name="contentRepository">The content repository for accessing content data.</param>
        public GetContentDetailHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the GetContentDetailQuery request by retrieving
        /// content details for the specified unique URL.
        /// </summary>
        /// <param name="request">The request containing the unique URL.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The generated content details for the specified URL.</returns>
        public async Task<GeneratedContentDto> Handle(GetContentDetailQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetGeneratedContentByUrlAsync(request.UniqUrl);
        }
    }
}
