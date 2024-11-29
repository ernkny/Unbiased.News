using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Cqrs.Queries
{
    /// <summary>
    /// Query to retrieve news without images, filtered by a specific start date.
    /// </summary>
    /// <param name="startDate">The start date to filter news by.</param>
    /// <returns>A collection of news without images, matching the filter criteria.</returns>
    public record GetNewsWithoutImagesQuery(DateTime startDate): IRequest<IEnumerable<GetNewsWithoutImageDto>>;
}
