using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Entities;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Categories
{
    /// <summary>
    /// Query record for retrieving all categories from the system.
    /// </summary>
    public record GetAllCategoriesQuery:IRequest<IEnumerable<Category>>;
}
