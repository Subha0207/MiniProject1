using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Exceptions.SubRouteExceptions;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubRouteController : ControllerBase
    {
        private readonly ISubRouteService _subrouteService;

        public SubRouteController(ISubRouteService subrouteService)
        {
            _subrouteService = subrouteService;

        }

        [HttpPost("AddSubRoute")]
        [ProducesResponseType(typeof(SubRouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubRouteReturnDTO[]>> AddSubRoute(SubRouteDTO[] subrouteDTO)
        {
            try
            {
                SubRouteReturnDTO[] returnDTO = await _subrouteService.AddSubRoutes(subrouteDTO);
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



        [HttpGet("GetAllSubRoutes")]
        [ProducesResponseType(typeof(List<SubRouteReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SubRouteReturnDTO>>> GetAllSubRoutes()
        {
            try
            {
                List<SubRouteReturnDTO> routes = await _subrouteService.GetAllSubRoutes();
                return Ok(routes);
            }
            catch (SubRouteException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (SubRouteServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

       

        [HttpGet("GetSubRoute/{subrouteId}")]
        [ProducesResponseType(typeof(SubRouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubRouteReturnDTO>> GetSubRoute(int subrouteId)
        {
            try
            {
                SubRouteReturnDTO route = await _subrouteService.GetSubRoute(subrouteId);
                return Ok(route);
            }
            catch (SubRouteException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (SubRouteServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }
        [HttpDelete("{subrouteId}")]
        [ProducesResponseType(typeof(SubRouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubRouteReturnDTO>> DeleteSubRoute(int subrouteId)
        {
            try
            {
                // Call the service to delete the subroute
                var deletedSubRoute = await _subrouteService.DeleteSubRoute(subrouteId);
                return Ok(deletedSubRoute);
            }
            catch (SubRouteNotFoundException ex)
            {
                // Subroute not found, return 404 Not Found with error message
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                // Other exceptions, return 500 Internal Server Error with error message
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        [HttpPut("UpdateSubRoute")]
        [ProducesResponseType(typeof(SubRouteReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubRouteReturnDTO>> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            try
            {
                SubRouteReturnDTO updatedSubRoute = await _subrouteService.UpdateSubRoute(subrouteReturnDTO);
                return Ok(updatedSubRoute);
            }
            catch (SubRouteException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (SubRouteServiceException ex)
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
