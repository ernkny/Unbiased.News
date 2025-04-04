using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to update the next scheduled run time for a search URL.
    /// </summary>
    /// <returns>A boolean value indicating whether the update was successful.</returns>
    public record UpdateSearchUrlNextRunTimeCommand : IRequest<bool>;
}
