using MediatR;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Application.Services
{
    public class ContentService : IContentService
    {
        private readonly IMediator _mediator;

        public ContentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail)
        {
            try
            {
                var result =await _mediator.Send(new InsertDailyHoroscopeCommand(horoscopeDetail));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
