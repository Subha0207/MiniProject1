using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IBookingService
    {/// <summary>
    /// Used to add booking by the user 
    /// </summary>
    /// <param name="bookingDTO"></param>
    /// <returns></returns>
        public Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO);
    /// <summary>
/// Used to get all the booking by the admin
/// </summary>
/// <returns></returns>
        public Task<List<ReturnBookingDTO>> GetAllBookings();
    }
}
