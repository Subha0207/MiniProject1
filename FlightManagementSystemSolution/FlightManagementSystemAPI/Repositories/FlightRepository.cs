using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Repositories
{
    public class FlightRepository : IRepository<int, Flight>
    {
        private readonly FlightManagementContext _context;

        public FlightRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<Flight> Add(Flight item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;

            }
            catch (Exception ex)
            {
                throw new FlightException("Error while adding Flight", ex);
            }
        }

        public async Task<Flight> Delete(int key)
        {
            try
            {
                var flight = await Get(key);
                _context.Remove(flight);
                await _context.SaveChangesAsync(true);
                return flight;

            }
            catch (FlightNotFoundException ex)
            {
                throw new FlightException("Error occurred while deleting flight. Flight not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new FlightException("Error occurred while deleting flight.", ex);
            }

        }

        public async Task<Flight> Get(int key)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId == key);
            if (flight == null)
                throw new FlightNotFoundException("No such flight is present.");
            return flight;
        }

        public async Task<IEnumerable<Flight>> GetAll()
        {
            try
            {
                var flights =await _context.Flights.ToListAsync();
                if (flights.Count > 0)
                {
                    return flights;
                }
                throw new FlightNotFoundException("No Flights found");
            }
            catch (FlightNotFoundException ex)
            {
                throw new FlightException("Error occurred while getting flights. " + ex.Message, ex);
            }
            catch (Exception ex)
            {

                throw new FlightException("Error occurred while getting flights. " + ex.Message, ex);
            }
           
        }

            public async Task<Flight> Update(Flight item)
            {
            try
            {
                var flight = await Get(item.FlightId);
                _context.Entry(flight).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (FlightNotFoundException ex)
            {
                throw new FlightException("Error occurred while updating flights. " + ex.Message, ex);
            }
            catch (Exception ex)
            {

                throw new FlightException("Error occurred while updating flights. " + ex.Message, ex);
            }
        }
        }
    } 
