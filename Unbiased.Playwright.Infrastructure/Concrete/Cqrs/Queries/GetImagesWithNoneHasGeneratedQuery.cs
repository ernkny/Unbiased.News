using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries
{
    public record GetImagesWithNoneHasGeneratedQuery: IRequest<IEnumerable<GeneratedNewsWithNoneImageDto>>;
    
}
