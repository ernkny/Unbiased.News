using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record AddNewsCommand(InsertNewsDto addNewsDto) : IRequest<Guid>;
}
