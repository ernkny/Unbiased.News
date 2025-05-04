using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    /// <summary>
    /// Query to retrieve all non-generated subheadings.
    /// </summary>
    public record GetAllNoneGeneratedSubHeadingsQuery:IRequest<IEnumerable<ContentSubHeading>>;
}

