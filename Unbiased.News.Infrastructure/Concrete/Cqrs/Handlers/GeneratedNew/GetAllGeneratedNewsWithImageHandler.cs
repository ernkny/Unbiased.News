using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handles the GetAllGeneratedNewsWithImageQuery to retrieve a list of generated news with images.
    /// </summary>
    public class GetAllGeneratedNewsWithImageHandler : IRequestHandler<GetAllGeneratedNewsWithImageQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllGeneratedNewsWithImageHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository instance.</param>
        public GetAllGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetAllGeneratedNewsWithImageQuery to retrieve a list of generated news with images.
        /// </summary>
        /// <param name="request">The GetAllGeneratedNewsWithImageQuery instance.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of GenerateNewsWithImageDto instances.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllGeneratedNewsWithImageQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageAsync(request.categoryId,request.pageNumber, request.language);
        }
    }
}
