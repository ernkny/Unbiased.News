using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News
{
    /// <summary>
    /// Query record for retrieving generated news with image path based on request parameters.
    /// </summary>
    /// <param name="GetGeneratedNewsWithImagePathRequestDto">The request DTO containing parameters for retrieving generated news with image paths.</param>
    public record GetGeneratedNewsQuery(GetGeneratedNewsWithImagePathRequestDto GetGeneratedNewsWithImagePathRequestDto):IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
