using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Shared.Dtos.Concrete;

namespace Unbiased.Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly IContactService _contactService;

        public CustomerDashboardController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [Authorize(Policy = "Customer Get")]
        [HttpGet("/GetAllCustomerMessages")]
        public async Task<IActionResult> GetAllCustomerMessages(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _contactService.GetAllCustomerMessagesAsync(pageNumber, pageSize);
                var response = new ResponseDto<List<Contact>>
                {
                    IsSuccessful = result.Any(),
                    StatusCode = result.Any() ? 200 : 204,
                    Data = result.ToList()
                };

                return result.Any() ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Customer Get")]
        [HttpGet("/GetCustomerMessagesById")]
        public async Task<IActionResult> GetCustomerMessagesById(int id)
        {
            try
            {
                var result = await _contactService.GetCustomerMessagesByIdAsync(id);
                var response = new ResponseDto<Contact>
                {
                    IsSuccessful = result is not null,
                    StatusCode = result is not null ? 200 : 204,
                    Data = result
                };

                return result is not null ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }

        [Authorize(Policy = "Customer Delete")]
        [HttpPost("/DeleteCustomerMessagesById")]
        public async Task<IActionResult> DeleteCustomerMessagesById(int id)
        {
            try
            {
                var result = await _contactService.DeleteCustomerMessagesByIdAsync(id);
                var response = new ResponseDto<bool>
                {
                    IsSuccessful = result,
                    StatusCode = result ? 200 : 204,
                    Data = result
                };

                return result ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseDto<string>
                {
                    IsSuccessful = false,
                    StatusCode = 500,
                    Data = ex is not null ? ex.Message : "An error occurred while processing your request."
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}