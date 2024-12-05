using MediatR;
using Unbiased.Log.Domain.Entities;

namespace Unbiased.Log.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// Command for inserting an activity log.
    /// </summary>
    /// <remarks>
    /// This command is used to insert a new activity log into the system.
    /// </remarks>
    public record InsertActivityLogCommand(
        /// <summary>
        /// The activity log to be inserted.
        /// </summary>
        ActivityLog ActivityLog
    ) : IRequest<bool>;
}
