using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Repositories
{
    public class CancellationRepository : IRepository<int, Cancellation>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<CancellationRepository> _logger;

        public CancellationRepository(FlightManagementContext context, ILogger<CancellationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cancellation> Add(Cancellation item)
        {
            try
            {
                _logger.LogInformation("Adding new cancellation.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cancellation added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding cancellation.");
                throw new RouteException("Error while adding Cancellation", ex);
            }
        }

        public async Task<Cancellation> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting cancellation with key: {key}");
                var cancellation = await Get(key);
                _context.Remove(cancellation);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cancellation deleted successfully.");
                return cancellation;
            }
            catch (CancellationNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting cancellation. Cancellation not found.");
                throw new CancellationException("Error occurred while deleting cancellation. Cancellation not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting cancellation.");
                throw new CancellationException("Error occurred while deleting cancellation.", ex);
            }
        }

        public async Task<Cancellation> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting cancellation with CancellationId: {key}");
                var cancellation = await _context.Cancellations.FirstOrDefaultAsync(c => c.CancellationId == key);
                if (cancellation == null)
                {
                    _logger.LogWarning("No such cancellation is present.");
                    throw new RouteNotFoundException("No such cancellation is present.");
                }
                _logger.LogInformation("Cancellation details fetched successfully.");
                return cancellation;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting cancellation.");
                throw new CancellationException("Error while getting cancellation. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cancellation.");
                throw new CancellationException("Error while getting cancellation.", ex);
            }
        }

        public async Task<IEnumerable<Cancellation>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all cancellation details.");
                var cancellations = await _context.Cancellations.ToListAsync();
                if (cancellations.Count == 0)
                {
                    _logger.LogWarning("No cancellations found.");
                    throw new CancellationNotFoundException("No cancellation is present.");
                }
                _logger.LogInformation("Cancellation details fetched successfully.");
                return cancellations;
            }
            catch (CancellationNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting cancellations.");
                throw new RouteException("Error while getting cancellation. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cancellations.");
                throw new CancellationException("Error while getting cancellations.", ex);
            }
        }

        public async Task<Cancellation> Update(Cancellation item)
        {
            try
            {
                _logger.LogInformation($"Updating cancellation with CancellationId: {item.CancellationId}");
                var cancellation = await Get(item.CancellationId);
                _context.Entry(cancellation).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Cancellation updated successfully.");
                return item;
            }
            catch (CancellationNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while updating cancellation.");
                throw new CancellationException("Error while updating cancellation. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating cancellation.");
                throw new CancellationException("Error while updating cancellation. " + ex.Message, ex);
            }
        }
    }
}
