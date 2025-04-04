using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve all questions and answers associated with a specific match ID.
    /// </summary>
    /// <param name="MatchId">The unique identifier of the match to retrieve Q&A for.</param>
    public record GetAllQuestionsAndAnswerWithMatchIdForGeneratedNewQuery(string MatchId):IRequest<IEnumerable<QuestionAnswerDto>>;
}
