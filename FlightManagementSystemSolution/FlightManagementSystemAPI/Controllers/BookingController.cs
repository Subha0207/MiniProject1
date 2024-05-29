using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("addBooking")]
        public async Task<IActionResult> AddBooking([FromBody] BookingDTO bookingDTO)
        {
            try
            {
                var returnBookingDTO = await _bookingService.AddBooking(bookingDTO);
                return Ok(returnBookingDTO);
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
