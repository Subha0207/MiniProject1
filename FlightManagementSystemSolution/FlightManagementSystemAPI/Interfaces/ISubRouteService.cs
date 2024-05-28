using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ISubRouteService
    {

       /// <summary>
       /// To add SubRoute for the Flights which are having more than 1 stop
       /// </summary>
       /// <param name="subrouteDTO"></param>
       /// <returns></returns>
       public  Task<SubRouteReturnDTO> AddSubRoute(SubRouteDTO subrouteDTO);


       /// <summary>
       /// Used to delete subroute for route of flight by admin
       /// </summary>
       /// <param name="subrouteId"></param>
       /// <returns></returns>
       public Task<SubRouteReturnDTO> DeleteSubRoute(int subrouteId); 
       /// <summary>
       /// Used to get all sub routes for a given route
       /// </summary>
       /// <returns></returns>
       public Task<List<SubRouteReturnDTO>> GetAllSubRoutes();
       /// <summary>
       /// Used to get subroute for a given subroute id
       /// </summary>
       /// <param name="subrouteId"></param>
       /// <returns></returns>
       public Task<SubRouteReturnDTO> GetSubRoute(int subrouteId);
       /// <summary>
       /// Used to update the subroute details
       /// </summary>
       /// <param name="subrouteReturnDTO"></param>
       /// <returns></returns>
       public Task<SubRouteReturnDTO> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO);
    }
}
