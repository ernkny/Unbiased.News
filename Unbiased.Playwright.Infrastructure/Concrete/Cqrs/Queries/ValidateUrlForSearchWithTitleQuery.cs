using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    public record ValidateUrlForSearchWithTitleQuery(string title) : IRequest<bool>;

}
