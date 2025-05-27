using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News
{
    /// <summary>
    /// Query record for retrieving the total count of generated news with images based on request parameters.
    /// </summary>
    /// <param name="requestDto">The request DTO containing parameters for counting generated news with images.</param>
    public record GetAllGeneratedNewsWithImageCountQuery(GetGeneratedNewsWithImagePathRequestDto requestDto):IRequest<int>;
}
