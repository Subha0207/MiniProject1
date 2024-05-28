using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Flight> _flightRepository;

        public RouteService(IRepository<int, FlightRoute> routeRepository, IRepository<int, Flight> flightRepository)
        {
            
            _routeRepository = routeRepository;
            _flightRepository = flightRepository;
        }

        public async Task<RouteReturnDTO> AddRoute(RouteDTO routeDTO)
        {
            try
            {

                var flight = await _flightRepository.Get(routeDTO.FlightId);
                if (flight == null)
                {
                    throw new FlightNotFoundException("No flight with the given ID exists.");
                }


                FlightRoute route = MapRouteDTOToRoute(routeDTO);

                FlightRoute AddedRoute = await _routeRepository.Add(route);
               
                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(AddedRoute);

                return routeReturnDTO;
            }

            catch (FlightNotFoundException fnf)
            {
                throw;
            }
            catch (RouteException fr)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Unable to add route", ex);
            }
        }

        public async Task<RouteReturnDTO> GetRoute(int routeId)
        {
            try
            {
                var route = await _routeRepository.Get(routeId);
                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(route);
                return routeReturnDTO;
            }
            catch (RouteException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new RouteServiceException("Unable to get the Route: " + e.Message, e);
            }
        }


        private RouteReturnDTO MapRouteToRouteReturnDTO(FlightRoute addedRoute)
        {
            RouteReturnDTO routeReturnDTO = new RouteReturnDTO();
            routeReturnDTO.RouteId = addedRoute.RouteId;
            routeReturnDTO.FlightId = addedRoute.FlightId;
            routeReturnDTO.ArrivalLocation = addedRoute.ArrivalLocation;
            routeReturnDTO.DepartureLocation = addedRoute.DepartureLocation;
            routeReturnDTO.ArrivalDateTime = addedRoute.ArrivalDateTime;
            routeReturnDTO.DepartureDateTime = addedRoute.DepartureDateTime;
            routeReturnDTO.PricePerPerson = addedRoute.PricePerPerson;
            routeReturnDTO.NoOfStops = addedRoute.NoOfStops;
            routeReturnDTO.SeatsAvailable = addedRoute.SeatsAvailable;
            return routeReturnDTO;
        }
        private FlightRoute MapRouteDTOToRoute(RouteDTO routeDTO)
        {
            
            FlightRoute route = new FlightRoute();
            route.FlightId = routeDTO.FlightId;
            route.ArrivalDateTime = routeDTO.ArrivalDateTime;   
            route.ArrivalLocation = routeDTO.ArrivalLocation;
            route.DepartureDateTime = routeDTO.DepartureDateTime;
            route.DepartureLocation = routeDTO.DepartureLocation;
            route.NoOfStops = routeDTO.NoOfStops;
            route.SeatsAvailable = routeDTO.SeatsAvailable;
            route.PricePerPerson=routeDTO.PricePerPerson;

            return route;
        }

        public async Task<RouteReturnDTO> DeleteRoute(int routeId)
        {
            try
            {
                var route = await _routeRepository.Delete(routeId);
                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(route);
                return routeReturnDTO;
            }
            catch (UnableToDeleteRouteException ex)
            {
                throw new RouteServiceException(ex.Message, ex);
            }
            catch (RouteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Unable to delete Route: " + ex.Message, ex);
            }

        }

        public async Task<List<RouteReturnDTO>> GetAllRoutes()
        {
            try
            {

                var routes = await _routeRepository.GetAll();

                List<RouteReturnDTO> routeReturnDTOs = new List<RouteReturnDTO>();
                foreach (FlightRoute route in routes)
                {
                    routeReturnDTOs.Add(MapRouteToRouteReturnDTO(route));
                }
                return routeReturnDTOs;

            }
            catch (RouteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Unable to get all Routes: " + ex.Message, ex);
            }

        }

        public Task<RouteReturnDTO> UpdateRoute(RouteReturnDTO routeReturnDTO)
        {
            throw new NotImplementedException();
        }
    }
}
