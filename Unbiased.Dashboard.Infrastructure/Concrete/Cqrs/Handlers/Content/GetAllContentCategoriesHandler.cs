using MediatR;
using Unbiased.Dashboard.Domain.Dto_s.Content;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    /// Handles the retrieval of all content categories from the database.
    /// </summary>
    public class GetAllContentCategoriesHandler : IRequestHandler<GetAllContentCategoriesQuery, IEnumerable<ContentCategories>>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllContentCategoriesHandler"/> class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public GetAllContentCategoriesHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        ///  Handles the request to get all content categories.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentCategories>> Handle(GetAllContentCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await  _contentRepository.GetAllContentCategoriesAsync();
        }
    }
}

