using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    public record GetAllActiveKeywordsForSearchQuery:IRequest<IEnumerable<string>>;
}
