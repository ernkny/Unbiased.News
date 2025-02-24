using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs
{
    public record UpdateBlogCommand(UpdateBlogDtoRequest UpdateBlogDtoRequest):IRequest<bool>;
}
