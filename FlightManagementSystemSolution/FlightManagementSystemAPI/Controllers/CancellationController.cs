using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Services;
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
        [HttpPost("addCancellation")]
        public async Task<IActionResult> AddCancellation([FromBody] CancellationDTO cancellationDTO)
        {
            try
            {
                var returnCancellationDTO = await _cancellationService.AddCancellation(cancellationDTO);
                return Ok(returnCancellationDTO);
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
