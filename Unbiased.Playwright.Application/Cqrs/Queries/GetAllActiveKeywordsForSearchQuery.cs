using MediatR;

namespace Unbiased.Playwright.Application.Cqrs.Queries
{
    public record GetAllActiveKeywordsForSearchQuery:IRequest<IEnumerable<string>>;
}
