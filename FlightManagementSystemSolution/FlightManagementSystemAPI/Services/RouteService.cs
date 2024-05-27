using FlightManagementSystemAPI.Exceptions;
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
                throw new RouteServiceException("Cannot add Route due to some error", ex);
            }
        }

        private RouteReturnDTO MapRouteToRouteReturnDTO(FlightRoute addedRoute)
        {
            RouteReturnDTO routeReturnDTO = new RouteReturnDTO();
            routeReturnDTO.RouteId = addedRoute.RouteId;
            return routeReturnDTO;
        }
        private FlightRoute MapRouteDTOToRoute(RouteDTO routeDTO)
        {
            
            FlightRoute route = new FlightRoute();
            route.FlightId = routeDTO.FlightId;
            route.ArrivalDate = routeDTO.ArrivalDate;
            route.ArrivalTime = routeDTO.ArrivalTime;
            route.ArrivalLocation = routeDTO.ArrivalLocation;
            route.DepartureDate = routeDTO.DepartureDate;
            route.DepartureTime = routeDTO.DepartureTime;
            route.DepartureLocation = routeDTO.DepartureLocation;
            route.NoOfStops = routeDTO.NoOfStops;
            route.SeatsAvailable = routeDTO.SeatsAvailable;



            return route;
        }
    }
}
