using MediatR;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// CQRS command for inserting a new content subheading into the database.
    /// </summary>
    /// <param name="id">The ID of the content category the subheading belongs to.</param>
    /// <param name="title">The title of the subheading to insert.</param>
    public record InsertContentSubheadingsCommand(int id,string title):IRequest<bool>;
}
