using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News
{
    /// <summary>
    /// Query record for retrieving a specific generated news item with image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the generated news item to retrieve.</param>
    public record GetGeneratedNewsByIdWithImageQuery(string id) : IRequest<GenerateNewsWithImageDto>;
}
