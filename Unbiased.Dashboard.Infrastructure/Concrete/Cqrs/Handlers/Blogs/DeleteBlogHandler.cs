using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Blogs;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Blogs
{
    internal class DeleteBlogHandler : IRequestHandler<DeleteBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository;

        public DeleteBlogHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
           return await _blogRepository.DeleteBlogByIdAsync(request.id);
        }
    }
}
