using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRefund([FromBody] RefundDTO refundDTO)
        {
            try
            {
                var returnRefundDTO = await _refundService.AddRefund(refundDTO);
                return Ok(returnRefundDTO);
            }
            catch (CancellationException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
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

        [HttpGet("{refundId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRefund(int refundId)
        {
            try
            {
                var returnrefundDTO = await _refundService.GetRefund(refundId);
                return Ok(returnrefundDTO);
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
        [HttpPut("UpdateRefund")]
        [ProducesResponseType(typeof(ReturnRefundDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnRefundDTO>> UpdateRefund(UpdateRefundDTO returnRefundDTO)
        {
            try
            {
                ReturnRefundDTO updatedRefund = await _refundService.UpdateRefund(returnRefundDTO);
                return Ok(updatedRefund);
            }
            catch (RefundNotFoundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                if (ex.InnerException is RefundNotFoundException)
                {
                    return BadRequest(new ErrorModel(400, ex.InnerException.Message));
                }
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet("GetAllPendingRefunds")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ReturnRefundDTO>>> GetAllPendingRefunds()
        {
            try
            {
                var initiatedRefunds = await _refundService.GetAllPendingRefunds();
                return Ok(initiatedRefunds);
            }
            catch (RefundNotFoundException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteRefund/{refundId}")]
        [ProducesResponseType(typeof(ReturnRefundDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnRefundDTO>> DeleteRefund(int refundId)
        {
            try
            {
                ReturnRefundDTO deletedRefund = await _refundService.DeleteRefundById(refundId);
                return Ok(deletedRefund);
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
