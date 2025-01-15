using MediatR;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    public class GetDailyHoroscopeDetailsHandler : IRequestHandler<GetDailyHoroscopeDetailsQuery, IEnumerable<HoroscopeDailyDetail>>
    {
        private readonly IContentRepository _contentRepository;

        public GetDailyHoroscopeDetailsHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<IEnumerable<HoroscopeDailyDetail>> Handle(GetDailyHoroscopeDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _contentRepository.GetDailyLastHoroscopesAsync();
            return result;
        }
    }
}
