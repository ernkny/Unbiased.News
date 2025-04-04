using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    /// <summary>
    /// Controller for managing contact form submissions and related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="contactService">The contact service instance for processing contact data.</param>
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Saves contact form information submitted by users.
        /// </summary>
        /// <param name="contact">The contact information to be saved.</param>
        /// <returns>A boolean value indicating whether the contact information was saved successfully.</returns>
        [HttpPost("/SaveContactFormInformations")]
        public async Task<IActionResult> SaveContactFormInformations([FromBody]Contact contact)
        {
            try
            {
                var result = await _contactService.SaveContact(contact);
                if (result == null)
                {

                    return NotFound(new ResponseDto<bool>
                    {
                        IsSuccessful = false,
                        StatusCode = 404,
                    });
                }

                var response = new ResponseDto<bool>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
