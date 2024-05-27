using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ISubRouteService
    {

        public  Task<SubRouteReturnDTO> AddSubRoute(SubRouteDTO subrouteDTO);
    }
}
