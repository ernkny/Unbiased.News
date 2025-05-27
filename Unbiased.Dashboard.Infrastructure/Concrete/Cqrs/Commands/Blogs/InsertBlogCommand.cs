using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs
{
    /// <summary>
    /// Command record for inserting a new blog with the specified blog data and user information.
    /// </summary>
    /// <param name="InsertBlogDtoRequest">The blog data transfer object containing the blog information to insert.</param>
    /// <param name="userId">The unique identifier of the user creating the blog.</param>
    public record InsertBlogCommand(InsertBlogDtoRequest InsertBlogDtoRequest,int userId) : IRequest<bool>;
}
