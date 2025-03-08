using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetAllGeneratedNewsWithImageCountQuery(int categoryId, string? title) :IRequest<int>; 
}
