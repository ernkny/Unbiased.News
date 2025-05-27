using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing GetGeneratedNewsByIdWithImageQuery requests to retrieve a specific generated news item with image.
    /// </summary>
    public class GetGeneratedNewsByIdWithImageHandler : IRequestHandler<GetGeneratedNewsByIdWithImageQuery, GenerateNewsWithImageDto>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetGeneratedNewsByIdWithImageHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public GetGeneratedNewsByIdWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetGeneratedNewsByIdWithImageQuery request and returns the specified generated news item with image.
        /// </summary>
        /// <param name="request">The query request containing the news ID to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing the generated news with image DTO.</returns>
        public async Task<GenerateNewsWithImageDto> Handle(GetGeneratedNewsByIdWithImageQuery request, CancellationToken cancellationToken)
        {
           return await _newsRepository.GetGeneratedNewsByIdWithImageAsync(request.id);
        }
    }
}
