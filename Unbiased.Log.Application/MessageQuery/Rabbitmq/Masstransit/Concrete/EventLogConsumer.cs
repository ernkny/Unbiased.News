using MassTransit;
using MediatR;
using Unbiased.Log.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Shared.ExceptionHandler.Middleware.Entities;

namespace Unbiased.Log.Application.MessageQuery.Rabbitmq.Masstransit.Concrete
{
    /// <summary>
    /// Consumer for handling EventLog messages.
    /// </summary>
    public class EventLogConsumer : IConsumer<EventLog>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogConsumer"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for sending commands.</param>
        public EventLogConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consumes the EventLog message and sends a command to insert it into the database.
        /// </summary>
        /// <param name="context">The consume context containing the EventLog message.</param>
        public async Task Consume(ConsumeContext<EventLog> context)
        {
            var eventLog = context.Message;

            try
            {
                await _mediator.Send(new InsertEventLogCommand(eventLog));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving EventLog: {ex.Message}");
            }
        }
    }
}
