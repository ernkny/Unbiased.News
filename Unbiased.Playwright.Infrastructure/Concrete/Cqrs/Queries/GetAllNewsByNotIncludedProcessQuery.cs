using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    public record GetAllNewsByNotIncludedProcessQuery:IRequest<IEnumerable<News>>;
}
