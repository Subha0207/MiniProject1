using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.PaymentExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using System;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Services
    {
        public class PaymentService : IPaymentService
        {
            private readonly IRepository<int, Payment> _paymentRepository;
            private readonly IRepository<int, Booking> _bookingRepository;
            private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger,IRepository<int, Payment> paymentRepository, IRepository<int, Booking> bookingRepository, IRepository<int, FlightRoute> routeRepository)
            {
                _paymentRepository = paymentRepository;
                _bookingRepository = bookingRepository;
                _routeRepository = routeRepository;
            _logger = logger;
        }
        #region AddPayment
        public async Task<ReturnPaymentDTO> AddPayment(PaymentDTO paymentDTO)
        {
            _logger.LogInformation("Adding payment...");

            var booking = await _bookingRepository.Get(paymentDTO.BookingId);
            if (booking == null)
            {
                _logger.LogError("Invalid booking ID.");
                throw new PaymentException("Invalid booking ID.");
            }

            // Retrieve the route associated with the booking
            var route = await _routeRepository.Get(booking.RouteId);
            if (route == null)
            {
                _logger.LogError("Invalid route ID.");
                throw new PaymentException("Invalid route ID.");
            }

            // Get the total amount from the booking
            var amount = booking.TotalAmount;

            // Create a new payment
            var newPayment = new Payment
            {
                BookingId = paymentDTO.BookingId,
                Amount = amount,
                PaymentMethod = paymentDTO.PaymentMethod,
            };
            if (newPayment.PaymentMethod == "")
            {
                throw new EmptyPaymentMethodException();
            }
            // Save the payment to the repository
            var savedPayment = await _paymentRepository.Add(newPayment);

            // Update the seats available for the route
            route.SeatsAvailable -= booking.NoOfPersons;
            await _routeRepository.Update(route);

            _logger.LogInformation("Payment added successfully.");

            // Return the payment details
            var returnPaymentDTO = new ReturnPaymentDTO
            {
                PaymentId = savedPayment.PaymentId,
                Amount = savedPayment.Amount
            };

            return returnPaymentDTO;
        }



        #endregion
        #region GetPaymentById
        public async Task<PaymentDetailsDTO> GetPayment(int PaymentId)
        {
            _logger.LogInformation($"Getting payment details for PaymentId: {PaymentId}");

            var payment = await _paymentRepository.Get(PaymentId);

            if (payment == null)
            {
                _logger.LogError($"Payment not found for PaymentId: {PaymentId}");
                throw new PaymentException("Payment not found.");
            }

            var paymentDetailsDTO = new PaymentDetailsDTO
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                BookingId = payment.BookingId,
                PaymentMethod = payment.PaymentMethod
            };

            _logger.LogInformation($"Successfully retrieved payment details for PaymentId: {PaymentId}");

            return paymentDetailsDTO;
        }

        #endregion
        #region GetAllPayments
        public async Task<List<PaymentDetailsDTO>> GetAllPayments()
        {
            _logger.LogInformation("GetAllPayments method called");
            try
            {
               List<Payment> payments = (List<Payment>)await _paymentRepository.GetAll();

                if (payments == null || payments.Count==0)
                {
                    _logger.LogError("No payments found");
                    throw new PaymentException("No payments found.");
                }

                var paymentDetailsDTOs = payments.Select(payment => new PaymentDetailsDTO
                {
                    PaymentId = payment.PaymentId,
                    Amount = payment.Amount,
                    BookingId = payment.BookingId,
                    PaymentMethod = payment.PaymentMethod
                }).ToList();

                _logger.LogInformation($"Successfully retrieved all payment details. Total payments: {paymentDetailsDTOs.Count}");

                return paymentDetailsDTOs;
            }
            catch (PaymentException ex)
            {
                _logger.LogError(ex, "PaymentException occurred while getting all payments");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting all payments");
                throw new PaymentException("Error Occurred While Getting All the Payments: " + ex.Message, ex);
            }
        }
        #endregion
        #region DeletePayment
        public async Task<ReturnPaymentDTO> DeletePaymentById(int paymentId)
        {
            try
            {
                _logger.LogInformation("Deleting payment...");
                Payment payment = await _paymentRepository.Delete(paymentId);
                if (payment == null)
                {
                    throw new PaymentNotFoundException("No payment with given ID exists");
                }
                ReturnPaymentDTO paymentReturnDTO = MapPaymentToReturnPaymentDTO(payment);
                _logger.LogInformation("Payment deleted successfully.");
                return paymentReturnDTO;
            }
            catch (PaymentNotFoundException)
            {
                throw;
            }
        
        }


        #endregion

        #region MapperMethods
        private ReturnPaymentDTO MapPaymentToReturnPaymentDTO(Payment payment)
        {
            ReturnPaymentDTO returnPaymentDTO = new ReturnPaymentDTO();
            returnPaymentDTO.PaymentId = payment.PaymentId;
            returnPaymentDTO.Amount = payment.Amount;
            return returnPaymentDTO;
        }

        #endregion
    }
}

