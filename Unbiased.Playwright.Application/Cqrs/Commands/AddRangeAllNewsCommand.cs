using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Application.Cqrs.Commands
{
    public record AddRangeAllNewsCommand(List<News> listOfNews):IRequest<bool>;
}
