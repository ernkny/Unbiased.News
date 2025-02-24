using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News
{
    public record UpdateGeneratedNewsWithImageCommand(UpdateGeneratedNewsDto generatedNewsDto) :IRequest<bool>;
}
