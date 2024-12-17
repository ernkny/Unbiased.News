using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    /// <summary>
    /// Handles the GetNewsWithoutImagesQuery by retrieving news items without images from the repository.
    /// </summary>
    public class GetNewsWithoutImagesQueryHandler : IRequestHandler<GetNewsWithoutImagesQuery, IEnumerable<GetNewsWithoutImageDto>>
    {
        // The news image repository instance used to perform the operation.
        private readonly INewsImageRepository _newsImageRepository;

        /// <summary>
        /// Initializes a new instance of the GetNewsWithoutImagesQueryHandler class.
        /// </summary>
        /// <param name="newsImageRepository">The news image repository to use for retrieving news items.</param>
        public GetNewsWithoutImagesQueryHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        /// <summary>
        /// Handles the GetNewsWithoutImagesQuery by retrieving news items without images from the repository.
        /// </summary>
        /// <param name="request">The GetNewsWithoutImagesQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of news items without images.</returns>
        public async Task<IEnumerable<GetNewsWithoutImageDto>> Handle(GetNewsWithoutImagesQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsImageRepository.GetNewsWithoutImagesAsync(request.startDate);
            return result;
        }
    }
}
