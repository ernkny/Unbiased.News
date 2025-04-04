using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to insert daily horoscope data into the database.
    /// </summary>
    /// <param name="horoscopeDetail">The entity containing the daily horoscope details.</param>
    /// <returns>A boolean value indicating whether the insertion was successful.</returns>
    public record InsertDailyHoroscopeCommand(HoroscopeDailyDetail horoscopeDetail):IRequest<bool>;
}
