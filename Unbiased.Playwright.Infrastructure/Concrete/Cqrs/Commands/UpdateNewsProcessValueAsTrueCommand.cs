using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record UpdateNewsProcessValueAsTrueCommand(string matchId) : IRequest<bool>;
}
