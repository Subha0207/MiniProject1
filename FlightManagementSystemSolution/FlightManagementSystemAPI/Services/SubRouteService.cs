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
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Flight> _flightRepository;

        public SubRouteService(IRepository<int, SubRoute> subrouteRepository, IRepository<int, Flight> flightRepository, IRepository<int, FlightRoute> routeRepository)
        {

            _subrouteRepository = subrouteRepository;
            _routeRepository = routeRepository;
            _flightRepository = flightRepository;


        }


        public async Task<SubRouteReturnDTO> AddSubRoute(SubRouteDTO subrouteDTO)
        {
            try
            {

                var flight = await _flightRepository.Get(subrouteDTO.FlightId);
                if (flight == null)
                {
                    throw new FlightNotFoundException("No flight with the given ID exists.");
                }

                var route = await _routeRepository.Get(subrouteDTO.RouteId);
                if (route == null)
                {
                    throw new RouteNotFoundException("No route  with the given ID exists.");
                }
        
                if (route.NoOfStops>0) 
                {
                    SubRoute subroute = MapSubRouteDTOToSubRoute(subrouteDTO);

                    SubRoute AddedRoute = await _subrouteRepository.Add(subroute);

                    SubRouteReturnDTO subrouteReturnDTO = MapSubRouteToSubRouteReturnDTO(AddedRoute);

                    return subrouteReturnDTO;
                }
                throw new SubRouteNotFoundException("No SubRoutes for the given route");
                
            }

            catch (SubRouteNotFoundException ex)
            {
                throw;
            }
            catch (SubRouteException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SubRouteServiceException("Cannot add SubRoute due to some error", ex);
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
