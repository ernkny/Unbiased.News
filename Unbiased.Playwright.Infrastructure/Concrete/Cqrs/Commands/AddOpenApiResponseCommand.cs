using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands
{
    public record AddOpenApiResponseCommand(string Response):IRequest;
}
