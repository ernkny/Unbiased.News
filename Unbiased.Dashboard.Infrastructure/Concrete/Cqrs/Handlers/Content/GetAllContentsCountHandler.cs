using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    /// Handles the retrieval of the total count of all contents based on specified criteria.
    /// </summary>
    public class GetAllContentsCountHandler:IRequestHandler<GetAllContentsCountQuery, int>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllContentsCountHandler"/> class.
        /// </summary>
        /// <param name="contentRepository"></param>
        /// <param name="serviceProvider"></param>
        public GetAllContentsCountHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        ///  Handles the request to get the total count of all contents.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(GetAllContentsCountQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetAllContentsCountAsync(request.Language,request.CategoryId,request.IsProcess);
        }
    }
}
