using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetAllGeneratedNewsForSiteMapHandler:IRequestHandler<GetAllGeneratedNewsForSiteMapQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;
        public GetAllGeneratedNewsForSiteMapHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllGeneratedNewsForSiteMapQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsForSiteMapAsync(request.language);
        }
    }
}
