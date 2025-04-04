using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to insert a question and answer pair into the database.
    /// </summary>
    /// <param name="QuestionAnswerDto">The data transfer object containing the question and answer details.</param>
    /// <returns>A boolean value indicating whether the insertion was successful.</returns>
    public record InsertQuestionAndAnswerCommand(QuestionAnswerDto QuestionAnswerDto) : IRequest<bool>;
}
