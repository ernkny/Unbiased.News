using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;

namespace Unbiased.News.Application.Services
{
    public sealed class ContentService : IContentService
    {
        private readonly IMediator _mediator;

        public ContentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync()
        {
            try
            {
                
                var getAllHoroscopeDailyInformation = await _mediator.Send(new GetDailyHoroscopeDetailsQuery());
                if (getAllHoroscopeDailyInformation.Any())
                {

                    return getAllHoroscopeDailyInformation;
                }
                return Enumerable.Empty<HoroscopeDailyDetail>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Contents> GetLastContentAsync()
        {
            try
            {

                var getDailyInformationContentQuery = await _mediator.Send(new GetDailyInformationContentQuery());
                if (getDailyInformationContentQuery is not null)
                {

                    return getDailyInformationContentQuery;
                }
                throw new Exception("No content found");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
