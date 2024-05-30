using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IBookingService
    {
        public Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO);

        public Task<List<ReturnBookingDTO>> GetAllBookings();
    }
}
