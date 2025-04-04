using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving news statistics information.
    /// </summary>
    public class GetNewsStatisticsHandler : IRequestHandler<GetNewsStatisticsQuery, StatisticsDto>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNewsStatisticsHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetNewsStatisticsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve news statistics information.
        /// </summary>
        /// <param name="request">The query request object.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A statistics DTO containing news system metrics.</returns>
        public async Task<StatisticsDto> Handle(GetNewsStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetNewsStatisticsAsync();
        }
    }
}
