using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IRouteService
    {

        public Task<RouteReturnDTO> AddRoute(RouteDTO routeDTO);
    }
}
