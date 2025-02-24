using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    public class UpdateGeneratedNewsWithImageHandler : IRequestHandler<UpdateGeneratedNewsWithImageCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        public UpdateGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(UpdateGeneratedNewsWithImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.UpdateGeneratedNewsWithImageAsync(request.generatedNewsDto);
        }
    }
}
