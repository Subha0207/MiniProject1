using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IBookingService
        {
        /// <summary>
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

        /// <summary>
        /// Get Booking details by using Booking id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ReturnBookingDTO> GetBookingById(int id);

        /// <summary>
        /// Delete Booking by using Booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public Task<ReturnBookingDTO> DeleteBookingById(int bookingId);
       
        /// <summary>
        /// Get All Bookings done by a particular user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  Task<List<ReturnBookingDTO>> GetAllBookingsByUserId(int userId);



    }
}
