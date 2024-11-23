using MediatR;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Cqrs.Queries
{
    public record GetNewsWithoutImagesQuery(DateTime startDate): IRequest<IEnumerable<GetNewsWithoutImageDto>>;
}
