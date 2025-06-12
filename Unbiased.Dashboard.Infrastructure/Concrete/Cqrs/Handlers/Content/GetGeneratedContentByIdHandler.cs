using MediatR;
using Unbiased.Dashboard.Domain.Dto_s.Content;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Content;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Content
{

    /// <summary>
    ///  Handles the retrieval of generated content by ID from the database.
    /// </summary>
    public class GetGeneratedContentByIdHandler : IRequestHandler<GetGeneratedContentByIdQuery, GeneratedContentDto>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGeneratedContentByIdHandler"/> class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public GetGeneratedContentByIdHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        ///  Handles the request to get generated content by ID.s
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GeneratedContentDto> Handle(GetGeneratedContentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contentRepository.GetGeneratedContentByIdAsync(request.Id);
        }
    }
}
