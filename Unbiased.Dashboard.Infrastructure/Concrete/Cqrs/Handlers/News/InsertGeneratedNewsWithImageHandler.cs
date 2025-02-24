using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    public class InsertGeneratedNewsWithImageHandler : IRequestHandler<InsertGeneratedNewsWithImageCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        public InsertGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(InsertGeneratedNewsWithImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.InsertGeneratedNewsWithImageAsync(request.newsWithImageDto);
        }
    }
}
