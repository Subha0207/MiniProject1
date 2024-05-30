using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
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

        

        [HttpPut("UpdateRefund")]
        [ProducesResponseType(typeof(ReturnRefundDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnRefundDTO>> UpdateRefund(ReturnRefundDTO returnRefundDTO)
        {
            try
            {
                ReturnRefundDTO updatedRefund = await _refundService.UpdateRefund(returnRefundDTO);
                return Ok(updatedRefund);
            }
            catch (RefundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

    }



}
