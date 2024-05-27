using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlightManagementSystemAPI.Repositories
{
    public class RouteRepository : IRepository<int, FlightRoute>
    {
        private readonly FlightManagementContext _context;

        public RouteRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<FlightRoute> Add(FlightRoute item)
        {
            try
            {
                 _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            
            catch (Exception ex)
            {
                throw new RouteException("Error while adding route",ex);
            }
        }

        public async Task<FlightRoute> Delete(int key)
        {
            try
            {
                var route = await Get(key);
                _context.Remove(route);
                await _context.SaveChangesAsync(true);
                return route;

            }
            catch (RouteNotFoundException ex)
            {
                throw new RouteException("Error occurred while deleting route. Route not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RouteException("Error occurred while deleting route.", ex);
            }
        }

        public async Task<FlightRoute> Get(int key)
        {
            try
            {

                var route = await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == key);
                if (route == null)
                    throw new RouteNotFoundException("No such route is present.");
                return route;
            }
            catch (RouteNotFoundException ex)
            {
                throw new RouteException("Error while getting route" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RouteException("Error while getting route", ex);
            }
        }

        public async Task<IEnumerable<FlightRoute>> GetAll()
        {
            try
            {

                var routes = await _context.Routes.ToListAsync();
                if (routes.Count<0)
                    throw new RouteNotFoundException("No route is present.");
                return routes;
            }
            catch (RouteNotFoundException ex)
            {
                throw new RouteException("Error while getting route" + ex.Message,ex);
            }
            catch (Exception ex)
            {
                throw new RouteException("Error while getting route", ex);
            }
        }

        public async Task<FlightRoute> Update(FlightRoute item)
        {
            try
            {
                var route = await Get(item.RouteId);
                _context.Update(item);
                await _context.SaveChangesAsync(true);
                return route;
            }
            catch (RouteNotFoundException ex)
            {
                throw new RouteException("Error while updating route" +ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RouteException("Error while updating route", ex);
            }
        }
    }
}
