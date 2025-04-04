using MediatR;
using Unbiased.News.Domain.DTOs;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve statistical information about news items in the system.
    /// </summary>
    public record GetNewsStatisticsQuery:IRequest<StatisticsDto>;
}
