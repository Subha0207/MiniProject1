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




        //    [HttpGet("GetAllFlightsRoutesAndSubroutes")]
        //    [ProducesResponseType(typeof(Dictionary<int, Dictionary<int, List<SubRoute>>>), StatusCodes.Status200OK)]
        //    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        //    public async Task<ActionResult<Dictionary<int, Dictionary<int, List<SubRoute>>>>> GetAllFlightsRoutesAndSubroutes()
        //    {
        //        try
        //        {
        //            var options = new JsonSerializerOptions
        //            {
        //                ReferenceHandler = ReferenceHandler.Preserve,
        //                MaxDepth = 64 // Adjust the max depth as needed
        //            };

        //            var flightsRoutesSubroutes = await _flightService.GetAllFlightsRoutesAndSubroutes();
        //            var json = JsonSerializer.Serialize(flightsRoutesSubroutes, options);
        //            return Ok(json);
        //        }
        //        catch (FlightException ex)
        //        {
        //            return StatusCode(500, new ErrorModel(500, ex.Message));
        //        }
        //        catch (FlightServiceException ex)
        //        {
        //            return StatusCode(500, new ErrorModel(500, ex.Message));
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new ErrorModel(500, ex.Message));
        //        }
        //    }
        //}

        [HttpGet("GetAllFlightsRoutesAndSubroutes")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetAllFlightsRoutesAndSubroutes()
        {
            try
            {
                var flightsRoutesSubroutes = await _flightService.GetAllFlightsRoutesAndSubroutes();
                var json = FormatNestedJson(flightsRoutesSubroutes);
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



        private string FormatNestedJson(Dictionary<int, Dictionary<int, List<SubRoute>>> flightsRoutesSubroutes)
        {
            var formattedData = new Dictionary<string, Dictionary<string, List<SubRoute>>>();

            foreach (var flightId in flightsRoutesSubroutes.Keys)
            {
                var routesAndSubroutes = new Dictionary<string, List<SubRoute>>();
                foreach (var routeId in flightsRoutesSubroutes[flightId].Keys)
                {
                    var subRoutes = flightsRoutesSubroutes[flightId][routeId];
                    routesAndSubroutes.Add($"Route: {routeId}", subRoutes);
                }
                formattedData.Add($"Flight: {flightId}", routesAndSubroutes);
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

