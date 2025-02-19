using MassTransit;
using MediatR;
using Unbiased.Log.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Shared.Extensions.Concrete.Entities;

namespace Unbiased.Log.Application.MessageQuery.Rabbitmq.Masstransit.Concrete
{
    /// <summary>
    /// Consumer for handling ActivityLog messages.
    /// </summary>
    public class ActivityLogConsumer : IConsumer<ActivityLog>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogConsumer"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for sending commands.</param>
        public ActivityLogConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consumes the ActivityLog message and sends a command to insert it into the database.
        /// </summary>
        /// <param name="context">The consume context containing the ActivityLog message.</param>
        public async Task Consume(ConsumeContext<ActivityLog> context)
        {
            var activityLog = context.Message;

            try
            {
                await _mediator.Send(new InsertActivityLogCommand(activityLog));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving EventLog: {ex.Message}");
            }
        }
    }
}
