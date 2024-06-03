using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPut("UpdateFlight")]
        [ProducesResponseType(typeof(FlightReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightReturnDTO>> UpdateFlight(FlightReturnDTO flightReturnDTO)
        {
            try
            {
                FlightReturnDTO updatedFlight = await _flightService.UpdateFlight(flightReturnDTO);
                return Ok(updatedFlight);
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
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteFlight/{flightId}")]
        [ProducesResponseType(typeof(FlightReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightReturnDTO>> DeleteFlight(int flightId)
        {
            try
            {
                FlightReturnDTO deletedFlight = await _flightService.DeleteFlight(flightId);
                return Ok(deletedFlight);
            }
            catch (FlightException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
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
        [Authorize(Roles = "admin")]
        [HttpGet("GetAllFlights")]
        [ProducesResponseType(typeof(List<FlightReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FlightReturnDTO>>> GetAllFlights()
        {
            try
            {
                List<FlightReturnDTO> flights = await _flightService.GetAllFlight();
                return Ok(flights);
            }
            catch (FlightException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
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


        [HttpGet("GetFlight/{flightId}")]
        [ProducesResponseType(typeof(FlightReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FlightReturnDTO>> GetFlight(int flightId)
        {
            try
            {
                FlightReturnDTO flight = await _flightService.GetFlight(flightId);
                return Ok(flight);
            }
            catch (FlightException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
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

        [HttpGet("GetAllFlightsRoutesAndSubroutes")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetAllFlightsRoutesAndSubroutes()
        {
            try
            {
                var flightsRoutesAndSubroutes = await _flightService.GetAllFlightsRoutesAndSubroutes();
                var json = FormatFlightsRoutesAndSubroutesJson(flightsRoutesAndSubroutes);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        private string FormatFlightsRoutesAndSubroutesJson(Dictionary<int, Dictionary<int, List<SubRouteDisplayDTO>>> flightsRoutesAndSubroutes)
        {
            var formattedData = new Dictionary<string, Dictionary<string, List<SubRouteDisplayDTO>>>();

            foreach (var flightId in flightsRoutesAndSubroutes.Keys)
            {
                var routesAndSubroutes = flightsRoutesAndSubroutes[flightId];
                var formattedRoutesAndSubroutes = new Dictionary<string, List<SubRouteDisplayDTO>>();

                foreach (var routeId in routesAndSubroutes.Keys)
                {
                    var subroutes = routesAndSubroutes[routeId];
                    formattedRoutesAndSubroutes.Add($"Route: {routeId}", subroutes);
                }

                formattedData.Add($"Flight: {flightId}", formattedRoutesAndSubroutes);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(formattedData, options);
        }







        [HttpGet("GetAllDirectFlights")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetAllRoutesOfFlight()
        {
            try
            {
                var routesOfFlight = await _flightService.GetAllDirectFlights();
                var json = FormatRoutesJson(routesOfFlight);
                return Ok(json);
            }
            catch (FlightException ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
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

        private string FormatRoutesJson(Dictionary<int, List<RouteDTO>> routesOfFlight)
        {
            var formattedData = new Dictionary<string, List<RouteDTO>>();

            foreach (var flightId in routesOfFlight.Keys)
            {
                var routes = routesOfFlight[flightId];
                formattedData.Add($"Flight: {flightId}", routes);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreReadOnlyProperties = true,
                IgnoreNullValues = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return JsonSerializer.Serialize(formattedData, options);
        }


    }
}

