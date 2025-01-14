using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record InsertDailyHoroscopeCommand(HoroscopeDailyDetail horoscopeDetail):IRequest<bool>;
}
