using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to update the news process value as true.
    /// </summary>
    /// <param name="matchId">The identifier of the news item to be updated.</param>
    /// <returns>A boolean indicating whether the update was successful.</returns>
    public record UpdateNewsProcessValueAsTrueCommand(string matchId) : IRequest<bool>;
}
