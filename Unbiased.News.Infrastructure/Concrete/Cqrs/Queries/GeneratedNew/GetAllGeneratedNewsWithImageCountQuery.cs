using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    public record GetAllGeneratedNewsWithImageCountQuery(int categoryId) :IRequest<int>; 
}
