using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {

        private readonly IRefundService _refundService;

        public RefundController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        [HttpPost("addRefund")]
        public async Task<IActionResult> AddRefund([FromBody] RefundDTO refundDTO)
        {
            try
            {
                var returnRefundDTO = await _refundService.AddRefund(refundDTO);
                return Ok(returnRefundDTO);
            }
            catch (RefundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Handle other exceptions (e.g., database errors) appropriately
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpGet("{refundId}")]
        public async Task<IActionResult> GetRefund(int refundId)
        {
            try
            {
                var returnrefundDTO = await _refundService.GetRefund(refundId);
                return Ok(returnrefundDTO);
            }
            catch (RefundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        }
}
