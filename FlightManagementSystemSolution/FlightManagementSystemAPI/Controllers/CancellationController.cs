using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancellationController : ControllerBase
    {
        private readonly ICancellationService _cancellationService;

        public CancellationController(ICancellationService cancellationService)
        {
            _cancellationService = cancellationService;
        }
        [HttpPost("AddCancellation")]
        public async Task<IActionResult> AddCancellation([FromBody] CancellationDTO cancellationDTO)
        {
            try
            {
                var returnCancellationDTO = await _cancellationService.AddCancellation(cancellationDTO);
                return Ok(returnCancellationDTO);
            }
            catch (PaymentNotFoundException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (BookingNotFoundException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }

        }

        [HttpGet("GetAllCancellations")]
        [ProducesResponseType(typeof(List<ReturnCancellationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ReturnCancellationDTO>>> GetAllCancellations()
        {
            try
            {
                List<ReturnCancellationDTO> cancellations = await _cancellationService.GetAllCancellations();
                return Ok(cancellations);
            }
            catch (NoCancellationExistsException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnCancellationDTO>> GetCancellationById(int id)
        {
            try
            {
                var cancellation = await _cancellationService.GetCancellationById(id);
                if (cancellation == null)
                {
                    return NotFound(new { message = $"Cancellation with ID {id} not found." });
                }
                return Ok(cancellation);
            }
            catch (CancellationNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteCancellationById/{cancellationId}")]
        [ProducesResponseType(typeof(ReturnCancellationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnCancellationDTO>> DeleteCancellationById(int cancellationId)
        {
            try
            {
                ReturnCancellationDTO returnDTO = await _cancellationService.DeleteCancellationById(cancellationId);
                if (returnDTO == null)
                {
                    return NotFound();
                }
                return Ok(returnDTO);
            }
            catch (CancellationException ex)
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
