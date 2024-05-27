using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<SubRouteReturnDTO>> AddSubRoute(SubRouteDTO subrouteDTO)
        {
            try
            {
                SubRouteReturnDTO returnDTO = await _subrouteService.AddSubRoute(subrouteDTO);
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
