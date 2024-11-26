using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Validators
{
    public class ValidateNewsScrapping : IValidateNewsScrapping
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMediator _mediator;

        public ValidateNewsScrapping(INewsRepository newsRepository, IMediator mediator)
        {
            _newsRepository = newsRepository;
            _mediator = mediator;
        }

        public async Task<bool> UB_sp_UrlValidateForSearchWithTitleAsync(string title)
        {
            return await _newsRepository.ValidateUrlForSearchWithTitleAsync(title);
        }
    }
}
