using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs
{
    /// <summary>
    /// Command record for deleting a blog by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the blog to delete.</param>
    public record DeleteBlogCommand(string id) : IRequest<bool>;
}
