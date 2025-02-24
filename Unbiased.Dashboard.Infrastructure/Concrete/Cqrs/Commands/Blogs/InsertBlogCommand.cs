using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs
{
    public record InsertBlogCommand(InsertBlogDtoRequest InsertBlogDtoRequest,int userId) : IRequest<bool>;
}
