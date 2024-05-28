using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Repositories
{
    public class BookingRepository : IRepository<int, Booking>
    {
        private readonly FlightManagementContext _context;

        public BookingRepository(FlightManagementContext context)
        {
            _context = context;
        }

        public async Task<Booking> Add(Booking item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;

            }
            catch(Exception ex)
            {
                throw new BookingException("Error while adding Booking", ex);

            }
        }

        public async Task<Booking> Delete(int key)
        {

            try
            {
                var booking = await Get(key);
                _context.Remove(booking);
                await _context.SaveChangesAsync(true);
                return booking;

            }
            catch (BookingNotFoundException ex)
            {
                throw new BookingException("Error occurred while deleting booking. Booking not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BookingException("Error occurred while deleting cancellation.", ex);
            }
        }

        public async Task<Booking> Get(int key)
        {
            try
            {
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == key);
                if (booking == null)
                {
                    throw new BookingNotFoundException("No booking exists with the given id");
                }
                return booking;
            }
            catch (BookingNotFoundException ex)
            {
                throw new BookingException("Error while getting booking" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BookingException("Error while getting Booking details", ex);
            }
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            try
            {
                var bookings = await _context.Bookings.ToListAsync();
                if (bookings.Count < 0)
                    throw new BookingNotFoundException("No booking is present.");
                return bookings;
            }
            catch (CancellationNotFoundException ex)
            {
                throw new BookingException("Error while getting bookings" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BookingException("Error while getting bookings", ex);
            }
        }

        public async Task<Booking> Update(Booking item)
        {
            try
            {
                var booking = await Get(item.BookingId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return booking;
            }
            catch (BookingNotFoundException ex)
            {
                throw new CancellationException("Error while updating  booking" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BookingException("Error while updating booking", ex);
            }
        }
    }
}
