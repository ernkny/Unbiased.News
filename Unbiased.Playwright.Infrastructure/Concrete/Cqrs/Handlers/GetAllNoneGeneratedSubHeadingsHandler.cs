using MediatR;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handler for the GetAllNoneGeneratedSubHeadingsQuery.
    /// </summary>
    public class GetAllNoneGeneratedSubHeadingsHandler : IRequestHandler<GetAllNoneGeneratedSubHeadingsQuery, IEnumerable<ContentSubHeading>>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllNoneGeneratedSubHeadingsHandler class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public GetAllNoneGeneratedSubHeadingsHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the GetAllNoneGeneratedSubHeadingsQuery request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubHeading>> Handle(GetAllNoneGeneratedSubHeadingsQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetAllNoneGeneratedSubHeadingsAsync(cancellationToken);
        }
    }
}
