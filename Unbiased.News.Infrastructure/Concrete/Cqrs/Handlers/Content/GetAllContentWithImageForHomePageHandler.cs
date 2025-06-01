using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    ///  Handler for the GetAllContentWithImageForHomePageQuery, responsible for retrieving content subheadings with images for the home page.
    /// </summary>
    public class GetAllContentWithImageForHomePageHandler : IRequestHandler<GetAllContentWithImageForHomePageQuery, IEnumerable<ContentSubHeadingWithImageDto>>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllContentWithImageForHomePageHandler class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public GetAllContentWithImageForHomePageHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Handles the GetAllContentWithImageForHomePageQuery to retrieve content subheadings with images.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<ContentSubHeadingWithImageDto>> Handle(GetAllContentWithImageForHomePageQuery request, CancellationToken cancellationToken)
        {
           return _contentRepository.GetAllContentWithImageForHomePageAsync(request.language);
        }
    }
}
