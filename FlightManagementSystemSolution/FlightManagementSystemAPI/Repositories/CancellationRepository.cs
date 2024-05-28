using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Repositories
{
    public class CancellationRepository : IRepository<int, Cancellation>
    {
        private readonly FlightManagementContext _context;

        public CancellationRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<Cancellation> Add(Cancellation item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }

            catch (Exception ex)
            {
                throw new RouteException("Error while adding Cancellation", ex);
            }
        }

        public async Task<Cancellation> Delete(int key)
        {
            try
            {
                var cancellation = await Get(key);
                _context.Remove(cancellation);
                await _context.SaveChangesAsync(true);
                return cancellation;

            }
            catch (CancellationNotFoundException ex)
            {
                throw new CancellationException("Error occurred while deleting cancellation. Cancellation not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CancellationException("Error occurred while deleting cancellation.", ex);
            }
        }

        public async Task<Cancellation> Get(int key)
        {
            try
            {
                var cancellation = await _context.Cancellations.FirstOrDefaultAsync(c=> c.CancellationId == key);
                if (cancellation == null)
                    throw new RouteNotFoundException("No such cancellation is present.");
                return cancellation;
            }
            catch (CancellationNotFoundException ex)
            {
                throw new CancellationException("Error while getting cancellation" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CancellationException("Error while getting cancellation", ex);
            }
        }

        public async Task<IEnumerable<Cancellation>> GetAll()
        {
            try
            {

                var cancellations = await _context.Cancellations.ToListAsync();
                if (cancellations.Count < 0)
                    throw new CancellationNotFoundException("No cancellation is present.");
                return cancellations;
            }
            catch (CancellationNotFoundException ex)
            {
                throw new RouteException("Error while getting cancellation" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CancellationException("Error while getting cancellation", ex);
            }
        }

        public async Task<Cancellation> Update(Cancellation item)
        {
            try
            {
                var cancellation = await Get(item.CancellationId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return cancellation;
            }
            catch (CancellationNotFoundException ex)
            {
                throw new CancellationException("Error while updating  cancellation" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CancellationException("Error while updating cancellation", ex);
            }
        }
    }
}
