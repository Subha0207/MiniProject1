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
        private readonly ILogger<CancellationService> _logger;
        public CancellationService(ILogger<CancellationService> logger,IRepository<int, Cancellation> cancellationRepository,IRepository<int, Booking> bookingRepository, IRepository<int, Payment> paymentRepository, IRepository<int,FlightRoute > routeRepository)
        {
            _cancellationRepository = cancellationRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _routeRepository = routeRepository;

            _logger = logger;
        }
        public async Task<ReturnCancellationDTO> AddCancellation(CancellationDTO cancellationDTO)
        {
            _logger.LogInformation("Starting method AddCancellation.");

            var booking = await _bookingRepository.Get(cancellationDTO.BookingId);
            if (booking == null)
            {
                _logger.LogError("No booking is present with the given ID.");
                throw new CancellationException("No booking is present with the given ID");
            }
            var route = await _routeRepository.Get(booking.RouteId);

            // Calculate the new number of available seats after cancellation
            var updatedSeatsAvailable = route.SeatsAvailable + booking.NoOfPersons;
            _logger.LogInformation($"Updated seats available: {updatedSeatsAvailable}");

            // Update the seats available for the route
            route.SeatsAvailable = updatedSeatsAvailable;
            await _routeRepository.Update(route);

            var newCancellation = new Cancellation
            {
                BookingId = cancellationDTO.BookingId,
                Reason = cancellationDTO.Reason,
                PaymentId = cancellationDTO.PaymentId
            };
            var savedCancellation = await _cancellationRepository.Add(newCancellation);

            // Return the booking details
            var returnCancellationDTO = new ReturnCancellationDTO
            {
                CancellationId = savedCancellation.CancellationId,
                Reason = savedCancellation.Reason
            };

            _logger.LogInformation("Successfully completed method AddCancellation.");
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
            _logger.LogInformation("Starting method GetAllCancellations.");
            try
            {
                var cancellations = await _cancellationRepository.GetAll();
                List<ReturnCancellationDTO> returnCancellationDTOs = new List<ReturnCancellationDTO>();
                foreach (var cancellation in cancellations)
                {
                    returnCancellationDTOs.Add(MapCancellationToReturnCancellationDTO(cancellation));
                }
                _logger.LogInformation("Successfully completed method GetAllCancellations.");
                return returnCancellationDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occurred While Getting All the Cancellations: {ex.Message}", ex);
                throw new CancellationException("Error Occurred While Getting All the Cancellations: " + ex.Message, ex);
            }
        }
        public async Task<ReturnCancellationDTO> GetCancellationById(int cancellationId)
        {
            _logger.LogInformation($"Starting method GetCancellationById for cancellationId: {cancellationId}.");
            try
            {
                var cancellation = await _cancellationRepository.Get(cancellationId);
                if (cancellation == null)
                {
                    _logger.LogError($"Cancellation with ID {cancellationId} not found.");
                    throw new CancellationException($"Cancellation with ID {cancellationId} not found.");
                }
                _logger.LogInformation($"Successfully retrieved cancellation with ID {cancellationId}.");
                return MapCancellationToReturnCancellationDTO(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting the cancellation with ID {cancellationId}: {ex.Message}", ex);
                throw new CancellationException($"Error occurred while getting the cancellation with ID {cancellationId}: {ex.Message}", ex);
            }
        }

        public async Task<ReturnCancellationDTO> DeleteCancellationById(int cancellationId)
        {
            try
            {
                var cancellation = await _cancellationRepository.Get(cancellationId);
                if (cancellation == null)
                {
                    throw new CancellationException($"Cancellation with ID {cancellationId} not found.");
                }

                await _cancellationRepository.Delete(cancellationId);

                _logger.LogInformation($"Cancellation with ID {cancellationId} has been successfully deleted.");

                return MapCancellationToReturnCancellationDTO(cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting the cancellation with ID {cancellationId}");
                throw new CancellationException($"Error occurred while deleting the cancellation with ID {cancellationId}: {ex.Message}", ex);
            }
        }

    }
}
