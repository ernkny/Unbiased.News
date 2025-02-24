using MediatR;

namespace Unbiased.Dashboard.Application.Services
{
    public class BlogService
    {
        private readonly IMediator _mediator;
        public BlogService(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
