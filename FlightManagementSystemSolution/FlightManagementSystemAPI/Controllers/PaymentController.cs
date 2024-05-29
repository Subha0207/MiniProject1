using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("addPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDTO paymentDTO)
        {
            try
            {
                var returnPaymentDTO = await _paymentService.AddPayment(paymentDTO);
                return Ok(returnPaymentDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Handle other exceptions (e.g., database errors) appropriately
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
