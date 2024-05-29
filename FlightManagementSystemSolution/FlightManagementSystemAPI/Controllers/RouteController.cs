using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Services;

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
        [HttpGet("GetAllRoutes")]
        [ProducesResponseType(typeof(List<RouteReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RouteReturnDTO>>> GetAllRoutes()
        {
            try
            {
                List<RouteReturnDTO> routes = await _routeService.GetAllRoutes();
                return Ok(routes);
            }
            catch (RouteException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
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

        [HttpDelete("DeleteRoute/{routeId}")]
        [ProducesResponseType(typeof(RouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteReturnDTO>> DeleteRoute(int routeId)
        {
            try
            {
                RouteReturnDTO route = await _routeService.DeleteRoute(routeId);
                return Ok(route);
            }
            catch (UnableToDeleteRouteException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (RouteException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
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

        [HttpGet("GetRoute/{routeId}")]
        [ProducesResponseType(typeof(RouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteReturnDTO>> GetRoute(int routeId)
        {
            try
            {
                RouteReturnDTO route = await _routeService.GetRoute(routeId);
                return Ok(route);
            }
            catch (RouteException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
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

        [HttpPut("UpdateRoute")]
        [ProducesResponseType(typeof(RouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteReturnDTO>> UpdateRoute(RouteReturnDTO routeReturnDTO)
        {
            try
            {
                RouteReturnDTO updatedRoute = await _routeService.UpdateRoute(routeReturnDTO);
                return Ok(updatedRoute);
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
