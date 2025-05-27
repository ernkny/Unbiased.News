using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News
{
    /// <summary>
    /// Command record for updating generated news with image data.
    /// </summary>
    /// <param name="generatedNewsDto">The generated news data transfer object containing updated information.</param>
    public record UpdateGeneratedNewsWithImageCommand(UpdateGeneratedNewsDto generatedNewsDto) :IRequest<bool>;
}
