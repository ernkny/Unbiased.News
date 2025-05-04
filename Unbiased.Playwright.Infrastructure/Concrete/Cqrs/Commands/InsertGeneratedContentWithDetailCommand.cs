using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// CQRS command for inserting comprehensive generated content with all its details into the database.
    /// </summary>
    /// <param name="ContentWithDetail">The complete content data with all associated details to insert.</param>
    public record InsertGeneratedContentWithDetailCommand(InsertAllContentDataRequest ContentWithDetail) :IRequest<bool>;
}
