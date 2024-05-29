using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Repositories
{
    public class RefundRepository : IRepository<int, Refund>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<RefundRepository> _logger;

        public RefundRepository(FlightManagementContext context, ILogger<RefundRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Refund> Add(Refund item)
        {
            try
            {
                _logger.LogInformation("Adding new refund.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Refund added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding refund.");
                throw new RefundException("Error while adding refund", ex);
            }
        }

        public async Task<Refund> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting refund with key: {key}");
                var refund = await Get(key);
                _context.Remove(refund);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Refund deleted successfully.");
                return refund;
            }
            catch (RefundNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting refund: Refund not found.");
                throw new RefundException("Error occurred while deleting refund. Refund not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting refund.");
                throw new RefundException("Error occurred while deleting refund.", ex);
            }
        }

        public async Task<Refund> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting refund with RefundId: {key}");
                var refund = await _context.Refunds.FirstOrDefaultAsync(rf => rf.RefundId == key);
                if (refund == null)
                {
                    _logger.LogWarning("No refund found with the given key.");
                    throw new RefundNotFoundException("No such refund is present.");
                }
                _logger.LogInformation("Refund details fetched successfully.");
                return refund;
            }
            catch (RefundNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting refund: " + ex.Message);
                throw new RefundException("Error while getting refund: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting refund.");
                throw new RefundException("Error while getting refund", ex);
            }
        }

        public async Task<IEnumerable<Refund>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all refunds.");
                var refunds = await _context.Refunds.ToListAsync();
                if (refunds.Count <= 0)
                {
                    _logger.LogWarning("No refunds found.");
                    throw new RefundNotFoundException("No refund is present.");
                }
                _logger.LogInformation("Refunds fetched successfully.");
                return refunds;
            }
            catch (RefundNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting refunds: " + ex.Message);
                throw new RefundException("Error while getting refunds: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting refunds.");
                throw new RefundException("Error while getting refunds", ex);
            }
        }

        public async Task<Refund> Update(Refund item)
        {
            try
            {
                _logger.LogInformation($"Updating refund with RefundId: {item.RefundId}");
                var refund = await Get(item.RefundId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Refund updated successfully.");
                return refund;
            }
            catch (RefundNotFoundException ex)
            {
                _logger.LogError(ex, "Error while updating refund: " + ex.Message);
                throw new RefundException("Error while updating refund: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating refund.");
                throw new RefundException("Error while updating refund", ex);
            }
        }
    }
}
