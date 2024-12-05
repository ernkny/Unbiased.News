using MassTransit;

namespace Unbiased.Log.Application.MessageQuery.Rabbitmq.Masstransit.Abstract
{
    /// <summary>
    /// Defines a consumer interface for handling messages of type <typeparamref name="TMessage"/>.
    /// </summary>
    /// <typeparam name="TMessage">The type of message being consumed, which must be a reference type.</typeparam>
    public interface IConsumer<in TMessage> : IConsumer
        where TMessage : class
    {
        /// <summary>
        /// Consumes the message in the provided context.
        /// </summary>
        /// <param name="context">The context in which the message is being consumed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Consume(ConsumeContext<TMessage> context);
    }
}
