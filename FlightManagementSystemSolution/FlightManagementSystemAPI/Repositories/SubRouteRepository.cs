using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Exceptions.SubRouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Repositories
{
    public class SubRouteRepository : IRepository<int, SubRoute>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<SubRouteRepository> _logger;

        public SubRouteRepository(FlightManagementContext context, ILogger<SubRouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region AddSubRoute
        public async Task<SubRoute> Add(SubRoute item)
        {
            try
            {
                _logger.LogInformation("Adding new sub-route.");
                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Sub-route added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding sub-route.");
                throw new SubRouteException("Error while adding sub-route", ex);
            }
        }
        #endregion
        #region DeleteSubRoute
        public async Task<SubRoute> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting sub-route with key: {key}");
                var subRoute = await Get(key);
                if (subRoute == null)
                {
                    throw new SubRouteNotFoundException("No subroute exists with given id");
                }
                _context.Remove(subRoute);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Sub-route deleted successfully.");
                return subRoute;
            }
            catch (SubRouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sub-route: SubRoute not found.");
                throw;  }
            
        }
        #endregion
        #region GetSubRoute
        public async Task<SubRoute> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting sub-route with SubRouteId: {key}");
                var subRoute = await _context.SubRoutes.FirstOrDefaultAsync(s => s.SubRouteId == key);
                if (subRoute == null)
                {
                    _logger.LogWarning("No sub-route found with the given key.");
                    throw new SubRouteNotFoundException("No such sub-route is present.");
                }
                _logger.LogInformation("Sub-route details fetched successfully.");
                return subRoute;
            }
            catch (SubRouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting sub-route: " + ex.Message);
                throw new SubRouteException("Error while getting sub-route: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting sub-route.");
                throw new SubRouteException("Error while getting sub-route", ex);
            }
        }
        #endregion
        #region GetAllSubRoute
        public async Task<IEnumerable<SubRoute>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all sub-routes.");
                var subRoutes = await _context.SubRoutes.ToListAsync();
                if (subRoutes.Count <= 0)
                {
                    _logger.LogWarning("No sub-routes found.");
                    throw new SubRouteNotFoundException("No sub-route is present.");
                }
                _logger.LogInformation("Sub-routes fetched successfully.");
                return subRoutes;
            }
            catch (SubRouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while getting sub-routes: " + ex.Message);
                throw new SubRouteException("Error while getting sub-routes: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting sub-routes.");
                throw new SubRouteException("Error while getting sub-routes", ex);
            }
        }
        #endregion
        #region UpdateSubRoute
        public async Task<SubRoute> Update(SubRoute item)
        {
            try
            {
                _logger.LogInformation($"Updating sub-route with SubRouteId: {item.SubRouteId}");
                var subRoute = await Get(item.SubRouteId);
                if (subRoute == null)
                {
                    
                        throw new SubRouteNotFoundException("No subroute with given id");
                    
                }
                _context.Entry(subRoute).State = EntityState.Detached;
                _context.Update(item);

                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Sub-route updated successfully.");
                return subRoute;
            }
            catch (SubRouteNotFoundException ex)
            {
                _logger.LogError(ex, "Error while updating sub-route: " + ex.Message);
                throw;
            }
          
        }
        #endregion
    }
}
