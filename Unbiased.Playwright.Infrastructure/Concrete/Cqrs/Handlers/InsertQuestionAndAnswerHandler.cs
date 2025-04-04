using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handler for the InsertQuestionAndAnswerCommand that persists question and answer data to the database.
    /// </summary>
    public class InsertQuestionAndAnswerHandler : IRequestHandler<InsertQuestionAndAnswerCommand, bool>
    {
        private readonly INewsRepository _newsRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertQuestionAndAnswerHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for database operations.</param>
        public InsertQuestionAndAnswerHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the InsertQuestionAndAnswerCommand by persisting the question and answer data to the database.
        /// </summary>
        /// <param name="request">The command containing the question and answer data to insert.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> Handle(InsertQuestionAndAnswerCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.InsertQuestionAndAnswerAsync(request.QuestionAnswerDto);
        }
    }
}
