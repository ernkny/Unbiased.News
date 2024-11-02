using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Services
{
    public class InsertNewsService : IInsertNewsService
    {
        private readonly IMediator _mediator;

        public InsertNewsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Guid> InsertNewsAsync(InsertNewsDto addNewsDto)
        {
            var result = await _mediator.Send(new InsertNewsQuery(addNewsDto));
            return result;
        }
    }
}
