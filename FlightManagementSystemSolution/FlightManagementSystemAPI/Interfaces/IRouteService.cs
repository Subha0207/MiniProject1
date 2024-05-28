using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IRouteService
    {

     /// <summary>
     /// Used to add route for a given flight by admin
     /// </summary>
     /// <param name="routeDTO"></param>
     /// <returns></returns>
     public Task<RouteReturnDTO> AddRoute(RouteDTO routeDTO);
     /// <summary>
     /// Used to delete route by admin
     /// </summary>
     /// <param name="routeId"></param>
     /// <returns></returns>
     public   Task<RouteReturnDTO> DeleteRoute(int routeId);
     /// <summary>
     /// Used to get all routes by admin
     /// </summary>
     /// <returns></returns>
     public  Task<List<RouteReturnDTO>> GetAllRoutes();
     /// <summary>
     /// Used to get route by route id
     /// </summary>
     /// <param name="routeId"></param>
     /// <returns></returns>
     public   Task<RouteReturnDTO> GetRoute(int routeId);
     /// <summary>
     /// Used to update route details by admin
     /// </summary>
     /// <param name="routeReturnDTO"></param>
     /// <returns></returns>
     public  Task<RouteReturnDTO> UpdateRoute(RouteReturnDTO routeReturnDTO);
    }
}
