using MediatR;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record AddRangeAllNewsCommand(List<News> listOfNews):IRequest<bool>;
}
