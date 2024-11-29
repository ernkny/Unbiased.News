using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to add a range of news items.
    /// </summary>
    /// <param name="listOfNews">The list of news items to be added.</param>
    /// <returns>A boolean indicating whether the operation was successful.</returns>
    public record AddRangeAllNewsCommand(List<News> listOfNews):IRequest<bool>;
}
