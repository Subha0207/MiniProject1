using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystemAPI.Repositories
{
    public class SubRouteRepository : IRepository<int, SubRoute>
    {
        private readonly FlightManagementContext _context;

        public SubRouteRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<SubRoute> Add(SubRoute item)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }

            catch (Exception ex)
            {
                throw new SubRouteException("Error while adding sub-route", ex);
            }
        }

        public async Task<SubRoute> Delete(int key)
        {
            try
            {
                var subRoute = await Get(key);
                _context.Remove(subRoute);
                await _context.SaveChangesAsync(true);
                return subRoute;

            }
            catch (SubRouteNotFoundException ex)
            {
                throw new SubRouteException("Error occurred while deleting sub-route. SubRoute not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SubRouteException("Error occurred while deleting sub-route.", ex);
            }
        }

        public async Task<SubRoute> Get(int key)
        {
            try
            {

                var subRoute = await _context.SubRoutes.FirstOrDefaultAsync(s =>s.SubRouteId == key);
                if (subRoute == null)
                    throw new RouteNotFoundException("No such sub-route is present.");
                return subRoute;
            }
            catch (SubRouteNotFoundException ex)
            {
                throw new SubRouteException("Error while getting sub-route" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SubRouteException("Error while getting sub-route", ex);
            }
        }

        public async Task<IEnumerable<SubRoute>> GetAll()
        {
            try
            {

                var subRoutes = await _context.SubRoutes.ToListAsync();
                if (subRoutes.Count < 0)
                    throw new SubRouteNotFoundException("No sub-route is present.");
                return subRoutes;
            }
            catch (SubRouteNotFoundException ex)
            {
                throw new SubRouteException("Error while getting sub-route" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SubRouteException("Error while getting sub-route", ex);
            }
        }

        public async Task<SubRoute> Update(SubRoute item)
        {
            try
            {
                var subRoute = await Get(item.SubRouteId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return subRoute;
            }
            catch (SubRouteNotFoundException ex)
            {
                throw new SubRouteException("Error while updating sub-route" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SubRouteException("Error while updating sub-route", ex);
            }
        }
    }
}
