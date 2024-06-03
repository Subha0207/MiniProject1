using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("AddRefund")]
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


        [Authorize(Roles = "admin")]
        [HttpPut("UpdateRefund")]
        [ProducesResponseType(typeof(ReturnRefundDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnRefundDTO>> UpdateRefund(UpdateRefundDTO returnRefundDTO)
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


        [Authorize(Roles = "admin")]
        [HttpGet("GetAllPendingRefunds")]
        public async Task<ActionResult<List<ReturnRefundDTO>>> GetAllPendingRefunds()
        {
            try
            {
                var initiatedRefunds = await _refundService.GetAllPendingRefunds();
                return Ok(initiatedRefunds);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log, return error response)
                return StatusCode(500, "An error occurred while fetching initiated refunds.");
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteRefund/{refundId}")]
        [ProducesResponseType(typeof(ReturnRefundDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnRefundDTO>> DeleteRefund(int refundId)
        {
            try
            {
                ReturnRefundDTO deletedRefund = await _refundService.DeleteRefundById(refundId);
                return Ok(deletedRefund);
            }
            catch (RefundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }



    }



}
