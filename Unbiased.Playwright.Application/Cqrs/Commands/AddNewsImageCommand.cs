using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Cqrs.Commands
{
    public record AddNewsImageCommand(InsertNewsImageDto addNewsImageDto) : IRequest<Guid>;
}
