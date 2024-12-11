using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
{
    public class GetAllGeneratedNewsHandler : IRequestHandler<GetAllGeneratedNewsQuery, IEnumerable<Entities.GeneratedNews>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllGeneratedNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<Entities.GeneratedNews>> Handle(GetAllGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsAsync();
        }
    }
}
