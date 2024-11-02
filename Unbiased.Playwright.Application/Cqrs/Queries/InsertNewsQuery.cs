using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Cqrs.Queries
{
    public record InsertNewsQuery(InsertNewsDto addNewsDto) : IRequest<Guid>;
}
