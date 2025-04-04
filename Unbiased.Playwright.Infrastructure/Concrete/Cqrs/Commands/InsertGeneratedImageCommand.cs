using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to insert a generated image for a news item into the database.
    /// </summary>
    /// <param name="ImageNews">The data transfer object containing the image details for the news item.</param>
    /// <returns>A boolean value indicating whether the insertion was successful.</returns>
    public record InsertGeneratedImageCommand(InsertNewsImageDto ImageNews):IRequest<bool>;
}
