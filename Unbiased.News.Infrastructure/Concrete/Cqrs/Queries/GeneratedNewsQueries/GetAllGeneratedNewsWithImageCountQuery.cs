using MediatR;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    public record GetAllGeneratedNewsWithImageCountQuery(int categoryId, string language) :IRequest<int>; 
}
