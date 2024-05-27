using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class SubRouteService : ISubRouteService
    {
        private readonly IRepository<int, SubRoute> _subrouteRepository;

        public SubRouteService(IRepository<int, SubRoute> subrouteRepository)
        {

            _subrouteRepository = subrouteRepository;
        }


        public async Task<SubRouteReturnDTO> AddSubRoute(SubRouteDTO subrouteDTO)
        {
            try
            {
                SubRoute subroute = MapSubRouteDTOToSubRoute(subrouteDTO);

                SubRoute AddedRoute = await _subrouteRepository.Add(subroute);

                SubRouteReturnDTO subrouteReturnDTO = MapSubRouteToSubRouteReturnDTO(AddedRoute);

                return subrouteReturnDTO;
            }
            catch (FlightException fr)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FlightServiceException("Cannot add SubRoute due to some error", ex);
            }
        }


        private SubRouteReturnDTO MapSubRouteToSubRouteReturnDTO(SubRoute subRoute)
        {
            SubRouteReturnDTO subrouteReturnDTO = new SubRouteReturnDTO();
            subrouteReturnDTO.SubRouteId = subRoute.SubRouteId;
            return subrouteReturnDTO;
        }
        private SubRoute MapSubRouteDTOToSubRoute(SubRouteDTO subrouteDTO)
        {

            SubRoute subroute = new SubRoute();
            subroute.FlightId = subrouteDTO.FlightId;
            subroute.RouteId = subrouteDTO.RouteId;
            subroute.ArrivalLocation = subrouteDTO.ArrivalLocation;
            subroute.ArrivalDateTime = subrouteDTO.ArrivalDateTime;
            subroute.DepartureDateTime = subrouteDTO.DepartureDateTime;
            subroute.DepartureLocation = subrouteDTO.DepartureLocation;
            return subroute;

        }
    }
}
