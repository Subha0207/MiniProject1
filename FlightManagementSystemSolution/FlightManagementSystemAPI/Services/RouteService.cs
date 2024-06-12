using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, SubRoute> _subrouteRepository;
        private readonly IRepository<int, Flight> _flightRepository;

        private readonly ILogger<RouteService> _logger;

        public RouteService(IRepository<int, FlightRoute> routeRepository, IRepository<int, Flight> flightRepository, ILogger<RouteService> logger, IRepository<int, SubRoute> subrouteRepository, IRepository<int, Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
            _routeRepository = routeRepository;
            _subrouteRepository = subrouteRepository;
            _flightRepository = flightRepository;
            _logger = logger;
        }

        #region MapperMethods
        private RouteReturnDTO MapRouteToRouteReturnDTO(FlightRoute addedRoute)
        {
            return new RouteReturnDTO
            {
                RouteId = addedRoute.RouteId,
                FlightId = addedRoute.FlightId,
                ArrivalLocation = addedRoute.ArrivalLocation,
                DepartureLocation = addedRoute.DepartureLocation,
                ArrivalDateTime = addedRoute.ArrivalDateTime,
                DepartureDateTime = addedRoute.DepartureDateTime,
                PricePerPerson = addedRoute.PricePerPerson,
                NoOfStops = addedRoute.NoOfStops,
                SeatsAvailable = addedRoute.SeatsAvailable
            };
        }

        private FlightRoute MapRouteDTOToRoute(RouteDTO routeDTO)
        {
            return new FlightRoute
            {
                FlightId = routeDTO.FlightId,
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureDateTime = routeDTO.DepartureDateTime,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                SeatsAvailable = routeDTO.SeatsAvailable,
                PricePerPerson = routeDTO.PricePerPerson
            };
        }
        #endregion
        #region AddRoute
        public async Task<RouteReturnDTO> AddRoute(RouteDTO routeDTO)
        {
            try
            {
                _logger.LogInformation("Adding a new route");

                var flight = await _flightRepository.Get(routeDTO.FlightId);
                if (flight == null)
                {
                    _logger.LogError("No flight with the given ID {FlightId} exists.", routeDTO.FlightId);
                    throw new FlightNotFoundException("No flight with the given ID exists.");
                }

                if (routeDTO.ArrivalLocation == routeDTO.DepartureLocation)
                {
                    _logger.LogError("Arrival location and departure location cannot be the same.");
                    throw new RouteException("Arrival location and departure location cannot be the same.");
                }
                if (routeDTO.PricePerPerson == 0)
                {
                    _logger.LogError("Price per person must be greater than zero.");
                    throw new RouteException("Please enter the amount");
                }

                // Add validation for arrival and departure date time
                if (routeDTO.ArrivalDateTime <= routeDTO.DepartureDateTime)
                {
                    _logger.LogError("Arrival date time must be greater than departure date time.");
                    throw new RouteException("Arrival date time must be greater than departure date time.");
                }

                FlightRoute route = MapRouteDTOToRoute(routeDTO);
                FlightRoute AddedRoute = await _routeRepository.Add(route);

                _logger.LogInformation("Route added successfully with ID {RouteId}", AddedRoute.RouteId);

                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(AddedRoute);
                return routeReturnDTO;
            }
            catch (FlightNotFoundException fnf)
            {
                _logger.LogError(fnf, "Flight not found");
                throw;
            }
            catch (RouteException fr)
            {
                _logger.LogError(fr, "Route validation failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add route");
                throw new RouteServiceException("Unable to add route", ex);
            }
        }
        #endregion
        #region GetRoute
        public async Task<RouteReturnDTO> GetRoute(int routeId)
        {
            try
            {
                _logger.LogInformation("Getting route with ID {RouteId}", routeId);
                var route = await _routeRepository.Get(routeId);
                if (route == null)
                {
                    _logger.LogError("No route with the given ID {RouteId} exists.", routeId);
                    throw new RouteException("No route with the given ID exists.");
                }
                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(route);
                return routeReturnDTO;
            }
            catch (RouteException re)
            {
                _logger.LogError(re, "Route validation failed");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get the Route");
                throw new RouteServiceException("Unable to get the Route: " + e.Message, e);
            }
        }
        #endregion
        #region DeleteRoute
        public async Task<RouteReturnDTO> DeleteRoute(int routeId)
        {
            try
            {
                _logger.LogInformation("Deleting route with ID {RouteId}", routeId);
                var route = await _routeRepository.Delete(routeId);
                if (route == null)
                {
                    _logger.LogError("No route with the given ID {RouteId} exists.", routeId);
                    throw new RouteServiceException("No route with the given ID exists.");
                }

                var subroutes = await _subrouteRepository.GetAll();
                // Check if any subroute is associated with the route
                foreach (var subroute in subroutes)
                {
                    if (subroute.RouteId == routeId)
                    {
                        var subrouteId = subroute.SubRouteId;
                        throw new RouteServiceException($"A subroute with id {subrouteId} exists with the given route ID. Cannot delete the route.");
                    }
                }

                var bookings = await _bookingRepository.GetAll();
                // Check if any subroute is associated with the route
                foreach (var booking in bookings)
                {
                    if (booking.RouteId == routeId)
                    {
                        _logger.LogInformation("Entered if clause");
                        var bookingId = booking.BookingId;
                        throw new BookingException($"A subroute with id {bookingId} exists with the given route ID. Cannot delete the route.");
                     
                    }
                    _logger.LogInformation("exited if clause");
                }

                RouteReturnDTO routeReturnDTO = MapRouteToRouteReturnDTO(route);
                _logger.LogInformation("Route with ID {RouteId} deleted successfully", routeId);
                return routeReturnDTO;
            }
            catch (BookingException ex)
            {
                _logger.LogWarning(ex, "Route not found.");
                throw;
            }
           
            catch (RouteServiceException ex)
            {
                _logger.LogError(ex, "Route service exception occurred.");
                throw;
            }
          
        }

        #endregion
        #region GetAllRoutes
        public async Task<List<RouteReturnDTO>> GetAllRoutes()
        {
            try
            {
                _logger.LogInformation("Getting all routes");
                var routes = await _routeRepository.GetAll();
                if (routes == null)
                {
                    throw new RouteNotFoundException("No route exists");
                }
                List<RouteReturnDTO> routeReturnDTOs = routes.Select(MapRouteToRouteReturnDTO).ToList();
                return routeReturnDTOs;
            }
            catch (RouteNotFoundException rnfe)
            {
                _logger.LogError(rnfe, "Route validation failed");
                throw;
            }
            catch (RouteException re)
            {
                _logger.LogError(re, "Route validation failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get all Routes");
                throw new RouteServiceException("Unable to get all Routes: " + ex.Message, ex);
            }
        }

        #endregion
        #region UpdateRoute
        public async Task<RouteReturnDTO> UpdateRoute(RouteReturnDTO routeReturnDTO)
        {
            try
            {
                _logger.LogInformation("Updating route with ID {RouteId}", routeReturnDTO.RouteId);

                var existingRoute = await _routeRepository.Get(routeReturnDTO.RouteId);
                if (existingRoute == null)
                {
                    _logger.LogError("No route with the given ID {RouteId} exists.", routeReturnDTO.RouteId);
                    throw new RouteNotFoundException("No route with the given ID exists.");
                }

                var flight = await _flightRepository.Get(routeReturnDTO.FlightId);
                if (flight == null)
                {
                    _logger.LogError("No flight with the given ID {FlightId} exists.", routeReturnDTO.FlightId);
                    throw new FlightNotFoundException("No flight with the given ID exists.");
                }

                if (routeReturnDTO.ArrivalLocation == routeReturnDTO.DepartureLocation)
                {
                    _logger.LogError("Arrival location and departure location cannot be the same.");
                    throw new RouteException("Arrival location and departure location cannot be the same.");
                }
                if (routeReturnDTO.PricePerPerson <= 0)
                {
                    _logger.LogError("Price per person must be greater than zero.");
                    throw new RouteException("Please enter a valid amount for price per person.");
                }

                if (routeReturnDTO.ArrivalDateTime <= routeReturnDTO.DepartureDateTime)
                {
                    _logger.LogError("Arrival date time must be greater than departure date time.");
                    throw new RouteException("Arrival date time must be greater than departure date time.");
                }

                // Map the updated fields from the DTO to the existing route
                existingRoute.FlightId = routeReturnDTO.FlightId;
                existingRoute.ArrivalDateTime = routeReturnDTO.ArrivalDateTime;
                existingRoute.ArrivalLocation = routeReturnDTO.ArrivalLocation;
                existingRoute.DepartureDateTime = routeReturnDTO.DepartureDateTime;
                existingRoute.DepartureLocation = routeReturnDTO.DepartureLocation;
                existingRoute.NoOfStops = routeReturnDTO.NoOfStops;
                existingRoute.SeatsAvailable = routeReturnDTO.SeatsAvailable;
                existingRoute.PricePerPerson = routeReturnDTO.PricePerPerson;

                // Update the route in the repository
                var updatedRoute = await _routeRepository.Update(existingRoute);

                _logger.LogInformation("Route with ID {RouteId} updated successfully", routeReturnDTO.RouteId);

                // Map the updated route to a DTO to return
                RouteReturnDTO updatedRouteReturnDTO = MapRouteToRouteReturnDTO(updatedRoute);
                return updatedRouteReturnDTO;
            }
            catch (FlightNotFoundException fnf)
            {
                _logger.LogError(fnf, "Flight not found");
                throw new FlightNotFoundException("No flight with the given ID exists.") ;
            }
            catch (RouteNotFoundException re)
            {
                _logger.LogError(re, "Route validation failed");
                throw;
            }
            catch (RouteException re)
            {
                _logger.LogError(re, "Route validation failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update route");
                throw new RouteServiceException("Unable to update route", ex);
            }
        }
        #endregion


    }
}
