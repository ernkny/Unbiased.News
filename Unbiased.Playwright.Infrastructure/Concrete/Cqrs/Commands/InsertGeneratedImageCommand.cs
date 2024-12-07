using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record InsertGeneratedImageCommand(InsertNewsImageDto ImageNews):IRequest<bool>;
}
