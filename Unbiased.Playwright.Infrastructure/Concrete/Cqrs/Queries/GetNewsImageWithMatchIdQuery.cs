using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    public record GetNewsImageWithMatchIdQuery(string MatchID) : IRequest<bool>;
}
