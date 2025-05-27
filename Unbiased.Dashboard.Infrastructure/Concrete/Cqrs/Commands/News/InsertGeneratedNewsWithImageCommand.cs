using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News
{
    /// <summary>
    /// Command record for inserting generated news with image data.
    /// </summary>
    /// <param name="newsWithImageDto">The news with image data transfer object containing the information to insert.</param>
    public record InsertGeneratedNewsWithImageCommand(InsertNewsWithImageDto newsWithImageDto) : IRequest<bool>;

}