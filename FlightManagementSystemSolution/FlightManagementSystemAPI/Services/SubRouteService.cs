using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Exceptions.SubRouteExceptions;
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
            _routeRepository   = routeRepository;
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
            catch (FlightNotFoundException)
            {
                throw;
            }
            catch (RouteNotFoundException)
            {
                throw;
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
            subrouteReturnDTO.FlightId = subRoute.FlightId;
            subrouteReturnDTO.ArrivalLocation = subRoute.ArrivalLocation;
            subrouteReturnDTO.ArrivalDateTime = subRoute.ArrivalDateTime;
            subrouteReturnDTO.DepartureLocation = subRoute.DepartureLocation;
            subrouteReturnDTO.DepartureDateTime = subRoute.DepartureDateTime;
            subrouteReturnDTO.RouteId = subRoute.RouteId;
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

        public async Task<SubRouteReturnDTO> DeleteSubRoute(int subrouteId)
        {
            try
            {
                var subroute = await _subrouteRepository.Delete(subrouteId);
                SubRouteReturnDTO routeReturnDTO = MapSubRouteToSubRouteReturnDTO(subroute);
                return routeReturnDTO;
            }
            catch (UnableToDeleteSubRouteException ex)
            {
                throw new SubRouteServiceException(ex.Message, ex);
            }
            catch (SubRouteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SubRouteServiceException("Unable to delete Route: " + ex.Message, ex);
            }
        }

        public async Task<List<SubRouteReturnDTO>> GetAllSubRoutes()
        {
            try
            {

                var subroutes = await _subrouteRepository.GetAll();

                List<SubRouteReturnDTO> subrouteReturnDTOs = new List<SubRouteReturnDTO>();
                foreach (SubRoute subroute in subroutes)
                {
                    subrouteReturnDTOs.Add(MapSubRouteToSubRouteReturnDTO(subroute));
                }
                return subrouteReturnDTOs;

            }
            catch (SubRouteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SubRouteServiceException("Unable to get all SubRoutes: " + ex.Message, ex);
            }

        }

        public async Task<SubRouteReturnDTO> GetSubRoute(int subrouteId)
        {
            try
            {
                var subroute = await _subrouteRepository.Get(subrouteId);
                SubRouteReturnDTO subrouteReturnDTO = MapSubRouteToSubRouteReturnDTO(subroute);
                return subrouteReturnDTO;
            }
            catch (SubRouteException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SubRouteServiceException("Unable to get the SubRoute: " + e.Message, e);
            }
        }

        public Task<SubRouteReturnDTO> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            throw new NotImplementedException();
        }
    }
}
