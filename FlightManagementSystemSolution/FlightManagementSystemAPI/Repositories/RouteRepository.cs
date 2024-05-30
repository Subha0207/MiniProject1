using FlightManagementSystemAPI.Contexts;
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
    public class RouteRepository : IRepository<int, FlightRoute>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<RouteRepository> _logger;

        public RouteRepository(FlightManagementContext context, ILogger<RouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FlightRoute> Add(FlightRoute item)
        {
            try
            {
                _logger.LogInformation("Adding new route.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Route added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding route.");
                throw new RouteException("Error while adding route", ex);
            }
        }

        public async Task<FlightRoute> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting route with key: {key}");
                var route = await Get(key);
                _context.Remove(route);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Route deleted successfully.");
                return route;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting route: Route not found.");
                throw new RouteException("Error occurred while deleting route. Route not found. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting route.");
                throw new RouteException("Error occurred while deleting route.", ex);
            }
        }

        public async Task<FlightRoute> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting route with RouteId: {key}");
                var route = await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == key);
                if (route == null)
                {
                    _logger.LogWarning("No route found with the given key.");
                    throw new RouteNotFoundException("No such route is present.");
                }
                _logger.LogInformation("Route details fetched successfully.");
                return route;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting route: " + ex.Message);
                throw new RouteException("Error while getting route: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting route.");
                throw new RouteException("Error while getting route", ex);
            }
        }

        public async Task<IEnumerable<FlightRoute>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all routes.");
                var routes = await _context.Routes.ToListAsync();
                if (routes.Count <= 0)
                {
                    _logger.LogWarning("No routes found.");
                    throw new RouteNotFoundException("No route is present.");
                }
                _logger.LogInformation("Routes fetched successfully.");
                return routes;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting routes: " + ex.Message);
                throw new RouteException("Error while getting routes: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting routes.");
                throw new RouteException("Error while getting routes", ex);
            }
        }

        public async Task<FlightRoute> Update(FlightRoute item)
        {
            try
            {
                _logger.LogInformation($"Updating route with RouteId: {item.RouteId}");
                var route = await Get(item.RouteId);
                _context.Entry(route).State = EntityState.Detached;
                _context.Update(item);
          
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Route updated successfully.");
                return route;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while updating route: " + ex.Message);
                throw new RouteException("Error while updating route: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating route.");
                throw new RouteException("Error while updating route", ex);
            }
        }
    }
}
