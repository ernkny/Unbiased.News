using MediatR;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    public class GetDailyInformationContentHandler : IRequestHandler<GetDailyInformationContentQuery, Contents>
    {
        private readonly IContentRepository _contentRepository;

        public GetDailyInformationContentHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<Contents> Handle(GetDailyInformationContentQuery request, CancellationToken cancellationToken)
        {
            var result= await _contentRepository.GetLastContentAsync();
            return result;
        }
    }
}
