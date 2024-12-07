using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Cqrs.Commands
{
    /// <summary>
    /// Represents a command to add an image to a news item.
    /// </summary>
    /// <param name="addNewsImageDto">The data transfer object containing the image details.</param>
    /// <returns>A unique identifier for the image.</returns>
    public record AddNewsImageCommand(InsertNewsImageDto addNewsImageDto) : IRequest<bool>;
}
