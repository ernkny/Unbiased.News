using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Content
{
    /// <summary>
    /// CQRS query that requests detailed content information based on a unique URL.
    /// </summary>
    /// <param name="UniqUrl">The unique URL identifier for the content.</param>
    public record GetContentDetailQuery(string UniqUrl): IRequest<GeneratedContentDto>;
}

