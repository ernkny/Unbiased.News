using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.ContactUs;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.ContactUs;

namespace Unbiased.Dashboard.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IMediator _mediator;

        public ContactService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> DeleteCustomerMessagesByIdAsync(int id)
        {
            try
            {
               return await _mediator.Send(new DeleteCustomerMessageCommand(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Contact>> GetAllCustomerMessagesAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _mediator.Send(new GetAllCustomerMessagesQuery(pageNumber,pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Contact> GetCustomerMessagesByIdAsync(int id)
        {
            try
            {
                return await _mediator.Send(new GetCustomerByIdQuery(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
