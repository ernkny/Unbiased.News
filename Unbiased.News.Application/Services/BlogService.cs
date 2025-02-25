using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Blog;

namespace Unbiased.News.Application.Services
{
    public class BlogService:IBlogService
    {
        private readonly IMediator _mediator;

        public BlogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async  Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(int pageNumber, string searchData)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsWithImageQuery(pageNumber, searchData));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllBlogsWithImageCountAsync(string? searchData)
        {
            try
            {
                return await _mediator.Send(new GetAllBlogsWithImageCountQuery(searchData));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string UniqUrl)
        {
            try
            {
                return await _mediator.Send(new GetBlogWithImageByUniqUrlQuery(UniqUrl));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
