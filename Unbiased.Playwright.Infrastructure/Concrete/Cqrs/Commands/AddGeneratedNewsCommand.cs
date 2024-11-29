using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to add a generated news article.
    /// </summary>
    /// <param name="News">The news article to be added.</param>
    public record AddGeneratedNewsCommand(News News):IRequest<bool>;
}
