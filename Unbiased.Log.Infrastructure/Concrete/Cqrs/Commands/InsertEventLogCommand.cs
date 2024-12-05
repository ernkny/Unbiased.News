using MediatR;
using Unbiased.Log.Domain.Entities;

namespace Unbiased.Log.Infrastructure.Concrete.Cqrs.Commands
{
    /// <summary>
    /// This record represents a command to insert an event log into the system.
    /// It is used to encapsulate the event log data and provide a way to send the command to the appropriate handler.
    /// </summary>
    /// <remarks>
    /// This class inherits from the IRequest interface, which is a marker interface indicating that this command is a request.
    /// The IRequest interface is often used to indicate that a command or query is a request and should be handled by a handler.
    /// </remarks>
    public record InsertEventLogCommand(EventLog eventLog) : IRequest<bool>
    {
        /// <summary>
        /// The event log data to be inserted.
        /// </summary>
        public EventLog EventLog { get; init; }
    }
}
