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

            public PaymentService(IRepository<int, Payment> paymentRepository, IRepository<int, Booking> bookingRepository, IRepository<int, FlightRoute> routeRepository)
            {
                _paymentRepository = paymentRepository;
                _bookingRepository = bookingRepository;
                _routeRepository = routeRepository;
            }

            public async Task<ReturnPaymentDTO> AddPayment(PaymentDTO paymentDTO)
            {
                var booking = await _bookingRepository.Get(paymentDTO.BookingId);
                if (booking == null)
                {
                    throw new PaymentException("Invalid booking ID.");
                }

                // Retrieve the route associated with the booking
                var route = await _routeRepository.Get(booking.RouteId);
                if (route == null)
                {
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

                // Save the payment to the repository
                var savedPayment = await _paymentRepository.Add(newPayment);

                // Update the seats available for the route
                route.SeatsAvailable -= booking.NoOfPersons;
                await _routeRepository.Update(route);

                // Return the payment details
                var returnPaymentDTO = new ReturnPaymentDTO
                {
                    PaymentId = savedPayment.PaymentId,
                    Amount = savedPayment.Amount
                };

                return returnPaymentDTO;
            }
        }
    }

