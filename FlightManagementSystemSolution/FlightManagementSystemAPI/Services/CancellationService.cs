using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using Microsoft.AspNetCore.Routing;

namespace FlightManagementSystemAPI.Services
{
    public class CancellationService : ICancellationService
    {
        private readonly IRepository<int, Cancellation> _cancellationRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;

        public CancellationService(IRepository<int, Cancellation> cancellationRepository,IRepository<int, Booking> bookingRepository, IRepository<int, Payment> paymentRepository, IRepository<int,FlightRoute > routeRepository)
        {
            _cancellationRepository = cancellationRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _routeRepository = routeRepository;


        }
        public async Task<ReturnCancellationDTO> AddCancellation(CancellationDTO cancellationDTO)
        {
            var booking = await _bookingRepository.Get(cancellationDTO.BookingId);
            if (booking == null)
            {
                throw new CancellationException("No booking is present with the given ID");
            }
            var route = await _routeRepository.Get(booking.RouteId);

            // Calculate the new number of available seats after cancellation
            var updatedSeatsAvailable = route.SeatsAvailable + booking.NoOfPersons;

            // Update the seats available for the route
            route.SeatsAvailable = updatedSeatsAvailable;
            await _routeRepository.Update(route);

            var newCancellation = new Cancellation
            {
            BookingId=cancellationDTO.BookingId,
            Reason=cancellationDTO.Reason,
            PaymentId=cancellationDTO.PaymentId           
            };
            var savedCancellation=await _cancellationRepository.Add(newCancellation);

            // Return the booking details
            var returnCancellationDTO = new ReturnCancellationDTO
            {
               CancellationId=savedCancellation.CancellationId,
                Reason=savedCancellation.Reason
               
            };
            return returnCancellationDTO;
        }
        private ReturnCancellationDTO MapCancellationToReturnCancellationDTO(Cancellation cancellation)
        {
            return new ReturnCancellationDTO
            {
                CancellationId = cancellation.CancellationId,
                Reason = cancellation.Reason
            };
        }
        public async Task<List<ReturnCancellationDTO>> GetAllCancellations()
        {
            try
            {
                var cancellations = await _cancellationRepository.GetAll();
                List<ReturnCancellationDTO> returnCancellationDTOs = new List<ReturnCancellationDTO>();
                foreach (var cancellation in cancellations)
                {
                    returnCancellationDTOs.Add(MapCancellationToReturnCancellationDTO(cancellation));
                }
                return returnCancellationDTOs;
            }
            catch (Exception ex)
            {
                throw new CancellationException("Error Occurred While Getting All the Cancellations: " + ex.Message, ex);
            }
        }
    }
}
