using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Engine
{
    /// <summary>
    /// Query record for retrieving all engine configurations from the system.
    /// </summary>
    public record GetAllEngineConfigurationsQuery:IRequest<IEnumerable<EngineConfigurationDto>>;
}
