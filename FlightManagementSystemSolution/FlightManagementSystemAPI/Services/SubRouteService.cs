using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Exceptions.SubRouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Services
{
    public class SubRouteService : ISubRouteService
    {
        private readonly IRepository<int, SubRoute> _subrouteRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Flight> _flightRepository;

        public SubRouteService(
            IRepository<int, SubRoute> subrouteRepository,
            IRepository<int, Flight> flightRepository,
            IRepository<int, FlightRoute> routeRepository)
        {
            _subrouteRepository = subrouteRepository;
            _routeRepository = routeRepository;
            _flightRepository = flightRepository;
        }

        public async Task<SubRouteReturnDTO[]> AddSubRoutes(SubRouteDTO[] subrouteDTOs)
        {
            List<SubRouteReturnDTO> subrouteReturnDTOs = new List<SubRouteReturnDTO>();

            foreach (var subrouteDTO in subrouteDTOs)
            {
                var flight = await _flightRepository.Get(subrouteDTO.FlightId);
                if (flight == null)
                {
                    throw new FlightNotFoundException("No flight with given id");
                }

                var route = await _routeRepository.Get(subrouteDTO.RouteId);
                if (route == null)
                {
                    throw new RouteNotFoundException("No route with given id");
                }

                if (route.NoOfStops > 0)
                {
                    var subroutes = MapSubRouteDTOToSubRoutes(subrouteDTO);
                    if (route.ArrivalLocation != subroutes.Last().ArrivalLocation)
                    {
                        throw new Exception("The arrival location of the route does not match with the last stop's arrival location");
                    }

                    // Validate the departure location of the route matches with the first stop's departure location
                    if (route.DepartureLocation != subroutes.First().DepartureLocation)
                    {
                        throw new Exception("The departure location of the route does not match with the first stop's departure location");
                    }
                    if (route.NoOfStops == 1 && subroutes.Length != 2)
                    {
                        throw new Exception("You need to add one more stop detail");
                    }
                    else if (route.NoOfStops == 2 && subroutes.Length != 3)
                    {
                        throw new Exception("You need to add two more stop details");
                    }

                    foreach (var subroute in subroutes)
                    {
                        SubRoute addedRoute = await _subrouteRepository.Add(subroute);
                        SubRouteReturnDTO subrouteReturnDTO = MapSubRouteToSubRouteReturnDTO(addedRoute);
                        subrouteReturnDTOs.Add(subrouteReturnDTO);
                    }
                }
            }

            return subrouteReturnDTOs.ToArray();
        }

        private SubRouteReturnDTO MapSubRouteToSubRouteReturnDTO(SubRoute subRoute)
        {
            SubRouteReturnDTO subrouteReturnDTO = new SubRouteReturnDTO
            {
                SubRouteId = subRoute.SubRouteId,
                FlightId = subRoute.FlightId,
                ArrivalLocation = subRoute.ArrivalLocation,
                ArrivalDateTime = subRoute.ArrivalDateTime,
                DepartureLocation = subRoute.DepartureLocation,
                DepartureDateTime = subRoute.DepartureDateTime,
                RouteId = subRoute.RouteId

            };

            return subrouteReturnDTO;
        }



        private SubRoute[] MapSubRouteDTOToSubRoutes(SubRouteDTO subrouteDTO)
        {
            List<SubRoute> subroutes = new List<SubRoute>();
            SubRoute previousSubroute = null;



            foreach (var stop in subrouteDTO.Stops)
            {
             

                if (stop.ArrivalLocation == stop.DepartureLocation)
                {
                    throw new SubRouteException("Arrival and departure location names must be different");
                }
                // Validate the departure time is before the arrival time
                if (stop.DepartureDateTime >= stop.ArrivalDateTime)
                {
                    throw new SubRouteException("Departure time must be before arrival time");
                }

                // Validate the arrival time of the current stop is after the departure time of the previous stop
                if (previousSubroute != null && stop.ArrivalDateTime <= previousSubroute.DepartureDateTime)
                {
                    throw new SubRouteException("Arrival time of the current stop must be after the departure time of the previous stop");
                }
                if (subrouteDTO.Stops.Length == 2 && subroutes[0].ArrivalLocation != subroutes[1].DepartureLocation)
                {
                    throw new SubRouteException("The arrival location of the first stop must be the same as the departure location of the second stop");
                }
                SubRoute subroute = new SubRoute
                {
                    FlightId = subrouteDTO.FlightId,
                    RouteId = subrouteDTO.RouteId,
                    ArrivalLocation = stop.ArrivalLocation,
                    ArrivalDateTime = stop.ArrivalDateTime,
                    DepartureLocation = stop.DepartureLocation,
                    DepartureDateTime = stop.DepartureDateTime,
                    SubFlightId = stop.SubFlightId
                };

                subroutes.Add(subroute);
                previousSubroute = subroute;
            }

            return subroutes.ToArray();
        }

        public async Task<List<SubRouteReturnDTO>> GetAllSubRoutes()
        {
            var subroutes = await _subrouteRepository.GetAll();
            return subroutes.Select(MapSubRouteToSubRouteReturnDTO).ToList();
        }

        public async Task<SubRouteReturnDTO> GetSubRoute(int subrouteId)
        {
            var subroute = await _subrouteRepository.Get(subrouteId);
            if (subroute == null)
            {
                throw new SubRouteNotFoundException("No subroute with given id");
            }
            return MapSubRouteToSubRouteReturnDTO(subroute);
        }

        public async Task<SubRouteReturnDTO> DeleteSubRoute(int subrouteId)
        {
            var subroute = await _subrouteRepository.Delete(subrouteId);
            if (subroute == null)
            {
                throw new SubRouteNotFoundException("No subroute with given id");
            }
            return MapSubRouteToSubRouteReturnDTO(subroute);
        }

        public async Task<SubRouteReturnDTO> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            var subroute = await _subrouteRepository.Update(MapSubRouteReturnDTOToSubRoute(subrouteReturnDTO));
            if (subroute == null)
            {
                throw new SubRouteNotFoundException("No subroute with given id");
            }
            return MapSubRouteToSubRouteReturnDTO(subroute);
        }

        private SubRoute MapSubRouteReturnDTOToSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            return new SubRoute
            {
                SubRouteId = subrouteReturnDTO.SubRouteId,
                FlightId = subrouteReturnDTO.FlightId,
                ArrivalLocation = subrouteReturnDTO.ArrivalLocation,
                ArrivalDateTime = subrouteReturnDTO.ArrivalDateTime,
                DepartureLocation = subrouteReturnDTO.DepartureLocation,
                DepartureDateTime = subrouteReturnDTO.DepartureDateTime,
                RouteId = subrouteReturnDTO.RouteId
            };
        }

    }
}
