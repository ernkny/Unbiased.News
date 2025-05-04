using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handler for inserting content subheadings into the database.
    /// </summary>
    public class InsertContentSubheadingsHandler : IRequestHandler<InsertContentSubheadingsCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertContentSubheadingsHandler"/> class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public InsertContentSubheadingsHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the insertion of content subheadings into the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(InsertContentSubheadingsCommand request, CancellationToken cancellationToken)
        {
            return await _contentRepository.InsertContentSubheadings(request.id, request.title);
        }
    }
}
