using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to add a new news item.
    /// </summary>
    /// <param name="addNewsDto">The data transfer object containing the news item details.</param>
    /// <returns>A unique identifier for the newly added news item.</returns>
    public record AddNewsCommand(InsertNewsDto addNewsDto) : IRequest<Guid>;
}
