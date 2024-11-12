using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
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
            var result = _newsRepository.AddRangeAllNews(request.listOfNews);
            return result;
        }
    }
}
