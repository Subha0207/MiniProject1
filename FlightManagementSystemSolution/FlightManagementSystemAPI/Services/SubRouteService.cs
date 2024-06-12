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
        private readonly ILogger<SubRouteService> _logger;
        private readonly IRepository<int, SubRoute> _subrouteRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Flight> _flightRepository;

        public SubRouteService(ILogger<SubRouteService> logger,
            IRepository<int, SubRoute> subrouteRepository,
            IRepository<int, Flight> flightRepository,
            IRepository<int, FlightRoute> routeRepository)
        {
            _logger = logger;
            _subrouteRepository = subrouteRepository;
            _routeRepository = routeRepository;
            _flightRepository = flightRepository;
        }

        #region MapperMethods
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
        #endregion
        #region AddSubRoutes
        public async Task<SubRouteReturnDTO[]> AddSubRoutes(SubRouteDTO[] subrouteDTOs)
        {
            List<SubRouteReturnDTO> subrouteReturnDTOs = new List<SubRouteReturnDTO>();

            foreach (var subrouteDTO in subrouteDTOs)
            {   
                try
                {
                    var flight = await _flightRepository.Get(subrouteDTO.FlightId);
                    if (flight == null)
                    {
                        _logger.LogError("No flight with given id: {FlightId}", subrouteDTO.FlightId);
                        throw new FlightNotFoundException("No flight with given id");
                    }

                    var route = await _routeRepository.Get(subrouteDTO.RouteId);
                    if (route == null)
                    {
                        _logger.LogError("No route with given id: {RouteId}", subrouteDTO.RouteId);
                        throw new RouteNotFoundException("No route with given id");
                    }
                    foreach (var stop in subrouteDTO.Stops)
                    {
                        int subFlightId = stop.SubFlightId;
                        var subflight = await _flightRepository.Get(subFlightId);
                        if(subflight == null)
                        {
                            throw new FlightNotFoundException("No flight with given id");
                        }
                    }

                    if (route.NoOfStops > 0)
                    {
                       var subroutes = MapSubRouteDTOToSubRoutes(subrouteDTO);
                        if (route.NoOfStops == 1 || route.NoOfStops == 2)
                        {
                            if (subroutes[0].ArrivalLocation != subroutes[1].DepartureLocation)
                            {
                                throw new Exception("The arrival location of the first sub route does not match with departure location of previous subroute");
                            }
                            if (subroutes[0].ArrivalDateTime >= subroutes[1].DepartureDateTime)
                            {
                                throw new Exception("The departure time of current subroute must be greater than the arrival time of the first sub route");
                            }

                            if (route.NoOfStops == 2)
                            {
                                if (subroutes[1].ArrivalLocation != subroutes[2].DepartureLocation)
                                {
                                    throw new Exception("The arrival location of the second sub route does not match with departure location of previous subroute");
                                }
                                if (subroutes[1].ArrivalDateTime >= subroutes[2].DepartureDateTime)
                                {
                                    throw new Exception("The departure time of current subroute must be greater than the arrival time of the second sub route");
                                }
                            }
                        }






                        if (route.ArrivalLocation != subroutes.Last().ArrivalLocation)
                        {
                            _logger.LogError("The arrival location of the route does not match with the last stop's arrival location");
                            throw new Exception("The arrival location of the route does not match with the last stop's arrival location");
                        }

                        // Validate the departure location of the route matches with the first stop's departure location
                        if (route.DepartureLocation != subroutes.First().DepartureLocation)
                        {
                            _logger.LogError("The departure location of the route does not match with the first stop's departure location");
                            throw new Exception("The departure location of the route does not match with the first stop's departure location");
                        }

                        if (route.ArrivalDateTime != subroutes.Last().ArrivalDateTime)
                        {
                            _logger.LogError("The arrival time of the route does not match with the last stop's arrival time");
                            throw new Exception("The arrival time of the route does not match with the last stop's arrival time");
                        }

                        // Validate the departure location of the route matches with the first stop's departure location
                        if (route.DepartureDateTime != subroutes.First().DepartureDateTime)
                        {
                            _logger.LogError("The departure time of the route does not match with the first stop's time ");
                            throw new Exception("The departure time of the route does not match with the first stop's departure time");
                        }
                       


                        if (route.NoOfStops == 1 && subroutes.Length != 2)
                        {
                            _logger.LogError("You need to add two stop details for a route with one stop");
                            throw new Exception("You need to add two stop details for a route with one stop");
                        }
                        else if (route.NoOfStops == 2 && subroutes.Length != 3)
                        {
                            _logger.LogError("You need to add three stop details for a route with two stops");
                            throw new Exception("You need to add three stop details for a route with two stops");
                        }

                        foreach (var subroute in subroutes)
                        {
                            SubRoute addedRoute = await _subrouteRepository.Add(subroute);
                            SubRouteReturnDTO subrouteReturnDTO = MapSubRouteToSubRouteReturnDTO(addedRoute);
                            subrouteReturnDTOs.Add(subrouteReturnDTO);
                        }
                    }
                }


                catch (RouteNotFoundException ex)
                {
                    _logger.LogError(ex, "An error occurred while adding subroutes.");
                    throw;
                }
                catch (FlightNotFoundException ex)
                {
                    _logger.LogError(ex, "An error occurred while adding subroutes.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding subroutes.");
                    throw;
                }
            }

            return subrouteReturnDTOs.ToArray();
        }
        #endregion
        #region GetAllSubRoutes
        public async Task<List<SubRouteReturnDTO>> GetAllSubRoutes()
        {
            try
            {
                var subroutes = await _subrouteRepository.GetAll();
                return subroutes.Select(MapSubRouteToSubRouteReturnDTO).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all subroutes.");
                throw;
            }
        }
        #endregion
        #region GetSubRouteById
        public async Task<SubRouteReturnDTO> GetSubRoute(int subrouteId)
        {
            try
            {
                var subroute = await _subrouteRepository.Get(subrouteId);
                if (subroute == null)
                {
                    _logger.LogError("No subroute with id {SubRouteId} found", subrouteId);
                    throw new SubRouteNotFoundException("No subroute with given id");
                }
                return MapSubRouteToSubRouteReturnDTO(subroute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting subroute with id {SubRouteId}", subrouteId);
                throw;
            }
        }
        #endregion
        #region DeleteSubRoute
        

        public async Task<SubRouteReturnDTO> DeleteSubRoute(int subrouteId)
        {
            try
            {
                var subroute = await _subrouteRepository.Delete(subrouteId);
                if (subroute == null)
                {
                    _logger.LogError("No subroute with id {SubRouteId} found", subrouteId);
                    throw new SubRouteNotFoundException("No subroute with given id");
                }
                return MapSubRouteToSubRouteReturnDTO(subroute);
            }
            catch (SubRouteNotFoundException)
            {
                throw;
            }
            catch (SubRouteException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting subroute with id {SubRouteId}", subrouteId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting subroute with id {SubRouteId}", subrouteId);
                throw;
            }
        }





        #endregion
        #region UpdateSubRoute
        public async Task<SubRouteReturnDTO> UpdateSubRoute(SubRouteReturnDTO subrouteReturnDTO)
        {
            try
            {
                var updatedSubroute = await _subrouteRepository.Update(MapSubRouteReturnDTOToSubRoute(subrouteReturnDTO));

                var flight = await _flightRepository.Get(updatedSubroute.SubFlightId);
                if (flight == null)
                {
                    throw new FlightNotFoundException("No flight with given id exists");
                }
                //var route = await _routeRepository.Get(updatedSubrout.SubRouteId);
                //if (route == null)
                //{
                //    throw new RouteNotFoundException("No route with given id exists");
                //}

                if (updatedSubroute == null)
                {
                    _logger.LogError("No subroute with id {SubRouteId} found", subrouteReturnDTO.SubRouteId);
                    throw new SubRouteNotFoundException("No subroute with given id");
                }

                // Fetch the updated entity again if necessary
                updatedSubroute = await _subrouteRepository.Get(updatedSubroute.SubRouteId);

                return MapSubRouteToSubRouteReturnDTO(updatedSubroute);
            }
            catch (FlightNotFoundException)
            {
                throw;
            }
            catch (RouteNotFoundException)
            {
                throw;
            }
            catch (SubRouteNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating subroute with id {SubRouteId}", subrouteReturnDTO.SubRouteId);
                throw;
            }
        }
        #endregion

    }
}
