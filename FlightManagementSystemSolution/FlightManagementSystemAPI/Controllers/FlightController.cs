using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightManagementSystemAPI.Exceptions;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;

        }
     
        [HttpPost("AddFlight")]
        [ProducesResponseType(typeof(FlightReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightReturnDTO>> AddFlight(FlightDTO flightDTO)
        {
            try
            {
                FlightReturnDTO returnDTO = await _flightService.AddFlight(flightDTO);
                return Ok(returnDTO);
            }
            catch (FlightException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (FlightServiceException ex)
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
