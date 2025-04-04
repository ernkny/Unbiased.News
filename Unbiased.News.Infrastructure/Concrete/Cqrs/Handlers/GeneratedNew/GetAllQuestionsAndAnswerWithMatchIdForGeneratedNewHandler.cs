using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving questions and answers associated with a specific match ID.
    /// </summary>
    public class GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewHandler : IRequestHandler<GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewQuery, IEnumerable<QuestionAnswerDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve questions and answers for a specific match.
        /// </summary>
        /// <param name="request">The query request containing the match ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of question and answer DTOs associated with the match.</returns>
        public async Task<IEnumerable<QuestionAnswerDto>> Handle(GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllQuestionsAndAnswerWithMatchIdAsync(request.MatchId);
        }
    }
}
