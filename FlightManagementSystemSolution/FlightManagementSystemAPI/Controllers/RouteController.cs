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
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;

        }

        [HttpPost("AddRoute")]
        [ProducesResponseType(typeof(RouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteReturnDTO>> AddRoute(RouteDTO routeDTO)
        {
            try
            {
                RouteReturnDTO returnDTO = await _routeService.AddRoute(routeDTO);
                return Ok(returnDTO);
            }
            catch (RouteException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (RouteServiceException ex)
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
