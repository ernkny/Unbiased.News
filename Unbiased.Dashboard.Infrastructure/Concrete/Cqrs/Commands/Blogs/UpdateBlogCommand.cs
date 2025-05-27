using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs
{
    /// <summary>
    /// Command record for updating an existing blog with new data.
    /// </summary>
    /// <param name="UpdateBlogDtoRequest">The blog data transfer object containing the updated blog information.</param>
    public record UpdateBlogCommand(UpdateBlogDtoRequest UpdateBlogDtoRequest):IRequest<bool>;
}
