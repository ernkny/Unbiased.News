using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNews
{
    public record GetAllGeneratedNewsWithImageQuery : IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
