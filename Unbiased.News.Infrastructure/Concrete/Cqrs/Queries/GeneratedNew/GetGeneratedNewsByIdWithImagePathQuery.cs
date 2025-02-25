using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetGeneratedNewsByIdWithImagePathQuery(string id):IRequest<GenerateNewsWithImageDto>;
}
