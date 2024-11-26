using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class AddRangeAllNewsHandler : IRequestHandler<AddRangeAllNewsCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        public AddRangeAllNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public Task<bool> Handle(AddRangeAllNewsCommand request, CancellationToken cancellationToken)
        {
            var result = _newsRepository.AddRangeAllNewsAsync(request.listOfNews);
            return result;
        }
    }
}
