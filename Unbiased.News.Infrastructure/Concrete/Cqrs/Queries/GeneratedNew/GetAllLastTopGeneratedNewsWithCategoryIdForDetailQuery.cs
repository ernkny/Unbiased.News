using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve the latest top news items from a specific category for detail pages.
    /// </summary>
    /// <param name="CategoryId">The category ID to retrieve news from.</param>
    /// <param name="Id">The ID of the news item to exclude from results.</param>
    /// <param name="Language">The language of the news items to retrieve.</param>
    public record GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery(int CategoryId, string Id, string Language) : IRequest<IEnumerable<GenerateNewsWithImageDto>>;
}
