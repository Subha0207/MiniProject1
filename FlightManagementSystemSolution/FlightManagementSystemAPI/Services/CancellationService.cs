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

        public CancellationService(IRepository<int, Cancellation> cancellationRepository,IRepository<int, Booking> bookingRepository)
        {
            _cancellationRepository = cancellationRepository;
            _bookingRepository = bookingRepository;

        }
        public async Task<ReturnCancellationDTO> AddCancellation(CancellationDTO cancellationDTO)
        {
            var booking = await _bookingRepository.Get(cancellationDTO.BookingId);
            if (booking == null)
            {
                throw new CancellationException("No booking is present with the given ID");
            }

            var newCancellation = new Cancellation
            {
            BookingId=cancellationDTO.BookingId,
            Reason=cancellationDTO.Reason
              
            };
            var savedCancellation=await _cancellationRepository.Add(newCancellation);

            // Return the booking details
            var returnCancellationDTO = new ReturnCancellationDTO
            {
               CancellationId=savedCancellation.CancellationId
            };
            return returnCancellationDTO;
        }
    }
}
