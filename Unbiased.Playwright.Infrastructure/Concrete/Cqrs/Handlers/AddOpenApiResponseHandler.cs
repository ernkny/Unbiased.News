using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class AddOpenApiResponseHandler : IRequestHandler<AddOpenApiResponseCommand>
    {
        private readonly INewsRepository _newsRepository;

        public AddOpenApiResponseHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task Handle(AddOpenApiResponseCommand request, CancellationToken cancellationToken)
        {
           await _newsRepository.AddOpenAiResponseAsync(request.Response);
        }
    }
}
