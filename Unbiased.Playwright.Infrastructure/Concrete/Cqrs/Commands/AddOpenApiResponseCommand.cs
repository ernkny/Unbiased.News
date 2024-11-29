using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to add an OpenAPI response.
    /// </summary>
    /// <param name="Response">The OpenAPI response to be added.</param>
    /// <returns>No return value.</returns>
    public record AddOpenApiResponseCommand(string Response):IRequest;
}
