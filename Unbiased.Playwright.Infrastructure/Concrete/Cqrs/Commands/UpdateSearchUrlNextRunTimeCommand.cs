using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record UpdateSearchUrlNextRunTimeCommand : IRequest;
}
