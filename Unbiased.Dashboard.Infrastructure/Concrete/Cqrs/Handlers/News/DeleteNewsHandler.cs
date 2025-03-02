using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    public class DeleteNewsHandler : IRequestHandler<DeleteGeneretedNewsCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        public DeleteNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(DeleteGeneretedNewsCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.DeleteNewsByIdAsync(request.id);
        }
    }
}
