using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{

    /// <summary>
    /// Handler for inserting generated content with detailed information into the repository.
    /// </summary>
    public class InsertGeneratedContentWithDetailHandler: IRequestHandler<InsertGeneratedContentWithDetailCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertGeneratedContentWithDetailHandler"/> class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public InsertGeneratedContentWithDetailHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the insertion of generated content with detailed information into the repository.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(InsertGeneratedContentWithDetailCommand request, CancellationToken cancellationToken)
        {
           return await _contentRepository.AddGeneratedContentAsync(request.ContentWithDetail);
        }
    }
}
