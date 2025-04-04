using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to insert daily content into the database.
    /// </summary>
    /// <param name="DailyContentDto">The data transfer object containing the daily content details.</param>
    /// <returns>A boolean value indicating whether the insertion was successful.</returns>
    public record InsertDailyContentCommand(Contents DailyContentDto) : IRequest<bool>;
}
