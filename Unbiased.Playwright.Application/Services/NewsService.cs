using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Services
{
    public sealed class NewsService : INewsService
    {
        private readonly IMediator _mediator;

        public NewsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto)
        {
            var result = await _mediator.Send(new AddNewsCommand(addNewsDto));
            return result;
        }
    }
}
