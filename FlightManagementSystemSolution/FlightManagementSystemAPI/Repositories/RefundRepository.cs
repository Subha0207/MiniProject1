using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Repositories
{
    public class RefundRepository : IRepository<int, Refund>
    {
        private readonly FlightManagementContext _context;

        public RefundRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<Refund> Add(Refund item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }

            catch (Exception ex)
            {
                throw new RefundException("Error while adding refund", ex);
            }
        }

        public async Task<Refund> Delete(int key)
        {
            try
            {
                var refund = await Get(key);
                _context.Remove(refund);
                await _context.SaveChangesAsync(true);
                return refund;

            }
            catch (RefundNotFoundException ex)
            {
                throw new RouteException("Error occurred while deleting refund. Refund not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RefundException("Error occurred while deleting refund.", ex);
            }
        }

        public async Task<Refund> Get(int key)
        {
            try
            {

                var refund = await _context.Refunds.FirstOrDefaultAsync(rf => rf.RefundId == key);
                if (refund == null)
                    throw new RefundNotFoundException("No such refund is present.");
                return refund;
            }
            catch (RefundNotFoundException ex)
            {
                throw new RefundException("Error while getting refund" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RefundException("Error while getting refund", ex);
            }
        }

        public async Task<IEnumerable<Refund>> GetAll()
        {
            try
            {

                var refunds = await _context.Refunds.ToListAsync();
                if (refunds.Count < 0)
                    throw new RefundNotFoundException("No refund is present.");
                return refunds;
            }
            catch (RefundNotFoundException ex)
            {
                throw new RefundException("Error while getting refund" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RefundException("Error while getting refund", ex);
            }
        }

        public async Task<Refund> Update(Refund item)
        {
            try
            {
                var refund = await Get(item.RefundId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return refund;
            }
            catch (RefundNotFoundException ex)
            {
                throw new RefundException("Error while updating refund" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RefundException("Error while updating refund", ex);
            }
        }
    }
}
