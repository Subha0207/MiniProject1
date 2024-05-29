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
                if (stop.ArrivalDateTime <= stop.DepartureDateTime)
                {
                    throw new Exception("Arrival date time must be greater than departure date time");
                }

                // Validate arrival and departure location names are different
                if (stop.ArrivalLocation == stop.DepartureLocation)
                {
                    throw new Exception("Arrival and departure location names must be different");
                }

                // Check if the arrival location of this subroute matches the departure location of the previous subroute
                if (previousSubroute != null && previousSubroute.DepartureLocation != stop.ArrivalLocation)
                {
                    throw new Exception("The arrival location of this subroute does not match the departure location of the previous subroute");
                }

                // Check if the departure time of this subroute is later than the arrival time of the previous subroute
                if (previousSubroute != null && previousSubroute.DepartureDateTime >= stop.ArrivalDateTime)
                {
                    throw new Exception("The departure time of this subroute is not later than the arrival time of the previous subroute");
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

        public Task<SubRouteReturnDTO> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            throw new NotImplementedException();
        }

        public Task<SubRouteReturnDTO> DeleteSubRoute(int subrouteId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubRouteReturnDTO>> GetAllSubRoutes()
        {
            throw new NotImplementedException();
        }

        public Task<SubRouteReturnDTO> GetSubRoute(int subrouteId)
        {
            throw new NotImplementedException();
        }
    }
}
