using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlightManagementSystemAPI.Repositories
{
    public class FlightRepository : IRepository<int, Flight>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<FlightRepository> _logger;

        public FlightRepository(FlightManagementContext context, ILogger<FlightRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region AddFlight
        public async Task<Flight> Add(Flight item)
        {
            try
            {
                _logger.LogInformation("Adding new flight.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Flight added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding flight.");
                throw new FlightException("Error occurred while adding flight.", ex);
            }

        }
        #endregion
        #region DeleteFlight
        public async Task<Flight> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting flight with key: {key}");
                var flight = await Get(key);
                if (flight == null)
                {
                    throw new FlightNotFoundException("No such flight exists.");
                }

                _context.Remove(flight);
                _logger.LogInformation("Flight deleted successfully.");
                await _context.SaveChangesAsync(true);
                return flight;
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogError(ex, "Error while deleting flight. Flight not found.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting flight.");
                throw new FlightException("Error while deleting flight.", ex);
            }
        }
        #endregion
        #region GetFlight
        public async Task<Flight> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting Flight with FlightId: {key}");
                var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId == key);
                if (flight == null)
                {
                    _logger.LogWarning("No such flight is present.");
                    throw new FlightNotFoundException("No such flight is found.");
                }
                _logger.LogInformation("Flight details fetched successfully.");
                return flight;
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting flight.");
                throw new FlightNotFoundException( ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting flight.");
                throw new FlightException("Error occurred while getting flight. " + ex.Message, ex);
            }
        }
        #endregion
        #region GetAllFlight
        public async Task<IEnumerable<Flight>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Getting all Flight Details");
                var flights =await _context.Flights.ToListAsync();
                if (flights.Count > 0)
                {
                    return flights;
                }
                throw new FlightNotFoundException("No Flights found");
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting flights.");
                throw new FlightException("Error occurred while getting flights. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting flights.");
                throw new FlightException("Error occurred while getting flights. " + ex.Message, ex);
            }
           
        }
        #endregion
        #region UpdateFlight
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
                _logger.LogError(ex, "Error while updating flights.");
                throw new FlightException("Error  while updating flights. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating flights.");
                throw new FlightException("Error  while updating flights. " + ex.Message, ex);
            }
        }
        #endregion
    }
} 
