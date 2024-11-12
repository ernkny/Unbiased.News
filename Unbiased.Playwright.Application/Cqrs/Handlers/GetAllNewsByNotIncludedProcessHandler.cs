using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    public class GetAllNewsByNotIncludedProcessHandler : IRequestHandler<GetAllNewsByNotIncludedProcessQuery, IEnumerable<News>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllNewsByNotIncludedProcessHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> Handle(GetAllNewsByNotIncludedProcessQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsRepository.GetAllNewsByNotIncludedProcessAsync();
            return result;
        }
    }
}
