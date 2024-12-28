using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetAllGeneratedNewsWithImageCountQuery(int categoryId) :IRequest<int>; 
}
