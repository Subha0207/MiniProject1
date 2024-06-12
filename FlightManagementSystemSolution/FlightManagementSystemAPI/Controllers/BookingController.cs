using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("User/AddBooking")]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBooking([FromBody] BookingDTO bookingDTO)
        {
            try
            {
                var returnBookingDTO = await _bookingService.AddBooking(bookingDTO);
                return Ok(returnBookingDTO);
            }

            catch (SeatsFilledException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (InvalidNumberOfPersonException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (FlightNotFoundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (RouteNotFoundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet("Admin/GetAllBookings")]
        [ProducesResponseType(typeof(List<ReturnBookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ReturnBookingDTO>>> GetAllBookings()
        {
            try
            {
                List<ReturnBookingDTO> bookings = await _bookingService.GetAllBookings();
                return Ok(bookings);
            }

            catch (BookingException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }
        [HttpGet("GetAllBookingsByUserId")]
        [ProducesResponseType(typeof(List<ReturnBookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ReturnBookingDTO>>> GetAllBookings([FromQuery] int userId)
        {
          try
            {
                List<ReturnBookingDTO> bookings = await _bookingService.GetAllBookingsByUserId(userId);
                return Ok(bookings);
            }
            catch (UserNotFoundException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (BookingException ex)
            {
               return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
               return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        [HttpGet("User/GetBookingById/{id}")]
        [ProducesResponseType(typeof(ReturnBookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnBookingDTO>> GetBookingById(int id)
        {
            try
            {
                ReturnBookingDTO returnDTO = await _bookingService.GetBookingById(id);
                if (returnDTO == null)
                {
                    return NotFound();
                }
                return Ok(returnDTO);
            }
          
            catch (BookingException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("Admin/DeleteBookingById/{bookingId}")]
        [ProducesResponseType(typeof(ReturnBookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnBookingDTO>> DeleteBookingById(int bookingId)
        {
            try
            {
                ReturnBookingDTO returnDTO = await _bookingService.DeleteBookingById(bookingId);
                if (returnDTO == null)
                {
                    return NotFound();
                }
                return Ok(returnDTO);
            }
            catch (BookingNotFoundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (BookingException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }


    }
}
