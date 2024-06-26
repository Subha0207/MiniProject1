﻿using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.PaymentExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("AddPayment")]
        [ProducesResponseType(typeof(ReturnPaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDTO paymentDTO)
        {
            try
            {
                var returnPaymentDTO = await _paymentService.AddPayment(paymentDTO);
                return Ok(returnPaymentDTO);
            }
            catch (PaymentException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (EmptyPaymentMethodException ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }


        [HttpGet("GetPayment/{paymentId}")]
        [ProducesResponseType(typeof(PaymentDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentDetailsDTO>> GetPayment(int paymentId)
        {
            try
            {
                PaymentDetailsDTO paymentDetails = await _paymentService.GetPayment(paymentId);
                return Ok(paymentDetails);
            }
            catch (PaymentException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }
       [Authorize(Roles = "admin")]
        [HttpGet("GetAllPayments")]
        [ProducesResponseType(typeof(List<PaymentDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PaymentDetailsDTO>>> GetAllPayments()
        {
            try
            {
                List<PaymentDetailsDTO> payments = await _paymentService.GetAllPayments();
                return Ok(payments);
            }
            catch (PaymentException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, ex.Message));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("DeletePayment/{paymentId}")]
        [ProducesResponseType(typeof(ReturnPaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReturnPaymentDTO>> DeletePayment(int paymentId)
        {
            try
            {
               
                ReturnPaymentDTO deletedPayment = await _paymentService.DeletePaymentById(paymentId);
                if (deletedPayment == null)
                {
                    return NotFound(new ErrorModel(404, $"Payment with ID: {paymentId} not found."));
                }
                return Ok(deletedPayment);
            }
            catch (PaymentNotFoundException ex)
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
