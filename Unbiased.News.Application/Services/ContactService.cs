using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Application.Validators;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Commands.ContactCommands;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    ///  Service for handling contact form submissions.
    /// </summary>
    public sealed class ContactService : IContactService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ContactService"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="serviceProvider"></param>
        public ContactService(IMediator mediator, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        ///  Saves a contact form submission asynchronously.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<bool> SaveContact(Contact contact)
        {
            try
            {
                var validator = new ContactValidator();
                var validatorResult = validator.Validate(contact);
                if (validatorResult.IsValid)
                {
                    var result = await _mediator.Send(new InsertContactFormCommand(contact));

                    return result;
                }
                else
                {
                    throw new Exception(validatorResult.Errors.First().ToString());
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
