using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Repositories
{
    public class BookingRepository : IRepository<int, Booking>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(FlightManagementContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        #region  AddBooking
        public async Task<Booking> Add(Booking item)
        {
            try
            {
                _logger.LogInformation("Adding new booking.");

               
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Booking added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding booking.");
                throw new BookingException("Error while adding Booking", ex);
            }
        }
        #endregion
        #region DeleteBooking
        public async Task<Booking> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting booking with key: {key}");
                var booking = await Get(key);
                if (booking == null)
                {
                    throw new BookingNotFoundException($"Booking not found.");
                }
                _context.Remove(booking);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Booking deleted successfully.");
                return booking;
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking. Booking not found.");
                
                throw;
            }
            
        }

        #endregion
        #region GetBooking
        public async Task<Booking> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting booking with BookingId: {key}");
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == key);
                if (booking == null)
                {
                    _logger.LogWarning("No booking exists with the given id.");
                    throw new BookingNotFoundException("No booking exists with the given id");
                }
                _logger.LogInformation("Booking details fetched successfully.");
                return booking;
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking.");
                throw new BookingException("Error while getting booking. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking.");
                throw new BookingException("Error while getting booking details", ex);
            }
        }
        #endregion
        #region GetAllBooking
        public async Task<IEnumerable<Booking>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all bookings.");
                var bookings = await _context.Bookings.ToListAsync();
                if (bookings.Count == 0)
                {
                    _logger.LogWarning("No bookings found.");
                    throw new BookingNotFoundException("No booking is present.");
                }
                _logger.LogInformation("Booking details fetched successfully.");
                return bookings;
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting bookings.");
                throw new BookingException("Error while getting bookings. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting bookings.");
                throw new BookingException("Error while getting bookings", ex);
            }
        }
        #endregion
        #region UpdateBooking
        public async Task<Booking> Update(Booking item)
        {
            try
            {
                _logger.LogInformation($"Updating booking with BookingId: {item.BookingId}");
                var booking = await Get(item.BookingId);
                _context.Entry(booking).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Booking updated successfully.");
                return booking;
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking.");
                throw new BookingException("Error while updating booking. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking.");
                throw new BookingException("Error while updating booking", ex);
            }
        }
        #endregion
    }
}
