using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
{
    /// <summary>
    /// Handles the GetAllGeneratedNewsQuery request.
    /// </summary>
    public class GetAllGeneratedNewsHandler : IRequestHandler<GetAllGeneratedNewsQuery, IEnumerable<Entities.GeneratedNews>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllGeneratedNewsHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository.</param>
        public GetAllGeneratedNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetAllGeneratedNewsQuery request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The generated news.</returns>
        public async Task<IEnumerable<Entities.GeneratedNews>> Handle(GetAllGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsAsync();
        }
    }
}
