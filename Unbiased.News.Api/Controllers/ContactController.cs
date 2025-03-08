using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.News.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

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
