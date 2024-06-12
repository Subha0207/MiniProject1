using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository<int, Flight> _flightRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, SubRoute> _subrouteRepository;
        private readonly ILogger<FlightService> _logger;
        public FlightService(IRepository<int, Flight> flightRepository, IRepository<int, FlightRoute> routeRepository, IRepository<int, SubRoute> subrouteRepository, ILogger<FlightService> logger)
        {
           
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _subrouteRepository = subrouteRepository;
            _logger = logger;
        }


        #region  MapperMethods
        private FlightReturnDTO MapFlightToFlightReturnDTO(Flight addedFlight)
        {
            FlightReturnDTO flightReturnDTO = new FlightReturnDTO();
            flightReturnDTO.FlightId = addedFlight.FlightId;
            flightReturnDTO.FlightName = addedFlight.FlightName;
            flightReturnDTO.SeatCapacity = addedFlight.SeatCapacity;
            return flightReturnDTO;
        }
        private Flight MapFlightDTOToFlight(FlightDTO flightDTO)
        {
            Flight flight = new Flight();
            flight.FlightName = flightDTO.FlightName;
            flight.SeatCapacity = flightDTO.SeatCapacity;
            return flight;
        }

        private Flight MapFlightReturnDTOWithFlight(FlightReturnDTO flightReturnDTO)
        {
            Flight flight = new Flight();
            flight.FlightId = flightReturnDTO.FlightId;
            flight.FlightName = flightReturnDTO.FlightName;
            flight.SeatCapacity = flightReturnDTO.SeatCapacity;
            return flight;
        }
        #endregion
        #region AddFlight
        public async Task<FlightReturnDTO> AddFlight(FlightDTO flightDTO)
        {
            try
            {
                if (flightDTO.SeatCapacity == 0)
                {
                    throw new InvalidSeatCapacityException("Seat capacity cannot be 0.");
                }


                _logger.LogInformation("Adding flight...");
                Flight flight = MapFlightDTOToFlight(flightDTO);
                Flight AddedFlight = await _flightRepository.Add(flight);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(AddedFlight);
                _logger.LogInformation("Flight added successfully.");
                return flightReturnDTO;
            }
            catch (InvalidSeatCapacityException isce)
            {
                throw;
            }
            catch (FlightException fr)
            {
                _logger.LogError(fr, "Flight exception occurred while adding flight.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding flight.");
                throw new FlightServiceException("Error Occurred,Unable to Add Flight", ex);
            }
        }
        #endregion
        #region UpdateFlight
        public async Task<FlightReturnDTO> UpdateFlight(FlightReturnDTO FlightReturnDTO)
        {
            try
            {

                if (FlightReturnDTO.SeatCapacity == 0)
                {
                    throw new InvalidSeatCapacityException("Seat capacity cannot be 0.");
                }
                _logger.LogInformation("Updating flight...");
                Flight flight = MapFlightReturnDTOWithFlight(FlightReturnDTO);
                Flight UpdatedFlight = await _flightRepository.Update(flight);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(UpdatedFlight);
                _logger.LogInformation("Flight updated successfully.");
                return flightReturnDTO;
            }

            catch (InvalidSeatCapacityException isce)
            {
                throw;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating flight.");
                throw new FlightServiceException("Error Occurred, Unable to Update Flight" + e.Message, e);
            }
        }
        #endregion
        #region DeleteFlight
        public async Task<FlightReturnDTO> DeleteFlight(int flightId)
        {
            try
            {
                _logger.LogInformation("Deleting flight...");

                // Get all routes
                var routes = await _routeRepository.GetAll();

                // Check if any route is associated with the flight
                foreach (var route in routes)
                {
                    if (route.FlightId == flightId)
                    {
                        throw new FlightServiceException("A route exists with the given flight ID. Cannot delete the flight.");
                    }
                }

                // If no associated routes found, proceed with flight deletion
                Flight flight = await _flightRepository.Delete(flightId);

                // If the flight is null, throw FlightNotFoundException
                if (flight == null)
                {
                    throw new FlightNotFoundException("No such flight exists.");
                }

                // Map the flight to a DTO
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(flight);

                _logger.LogInformation("Flight deleted successfully.");

                return flightReturnDTO;
            }
            catch (FlightNotFoundException fnf)
            {
                _logger.LogWarning(fnf, "Flight not found.");
                throw;  // Rethrow the exception to be caught by the controller
            }
            catch (FlightServiceException fse)
            {
                _logger.LogError(fse, "FlightServiceException occurred.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the flight.");
                throw new FlightServiceException("Error occurred while deleting the flight: " + ex.Message, ex);
            }
        }

        #endregion
        #region GetFlight
        public async Task<FlightReturnDTO> GetFlight(int flightId)
        {
            try
            {
                _logger.LogInformation("Fetching flight...");
                Flight flight = await _flightRepository.Get(flightId);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(flight);
                _logger.LogInformation("Flight fetched successfully.");
                return flightReturnDTO;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while fetching flight.");
                throw new FlightServiceException("Error Occurred, Unable to Get Flight" + e.Message, e);
            }
        }
        #endregion
        #region GetAllFlight
        public async Task<List<FlightReturnDTO>> GetAllFlight()
        {
            try
            {
                _logger.LogInformation("Fetching all flights...");
                var flights = await _flightRepository.GetAll();
                List<FlightReturnDTO> flightReturnDTOs = new List<FlightReturnDTO>();
                foreach (Flight flight in flights)
                {
                    flightReturnDTOs.Add(MapFlightToFlightReturnDTO(flight));
                }
                _logger.LogInformation("All flights fetched successfully.");
                return flightReturnDTOs;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all flights.");
                throw new FlightServiceException("Error Occurred While Getting All the Flight" + ex.Message, ex);
            }
        }
        #endregion
        #region GetAllFlightsRoutesAndSubroutes
        public async Task<Dictionary<int, Dictionary<int, List<SubRouteDisplayDTO>>>> GetAllFlightsRoutesAndSubroutes()
        {
            try
            {
                _logger.LogInformation("Fetching all flights, routes, and subroutes...");
                var flights = await _flightRepository.GetAll();
                var allRoutes = await _routeRepository.GetAll();
                var flightsRoutesAndSubroutes = new Dictionary<int, Dictionary<int, List<SubRouteDisplayDTO>>>();

                foreach (var flight in flights)
                {
                    var routesAndSubroutes = new Dictionary<int, List<SubRouteDisplayDTO>>();

                    foreach (var route in allRoutes)
                    {
                        if (route.FlightId == flight.FlightId && route.NoOfStops > 0)
                        {
                            var allSubroutes = await _subrouteRepository.GetAll();
                            var routeSubroutes = allSubroutes
                                .Where(subroute => subroute.RouteId == route.RouteId)
                                .Select(subroute => new SubRouteDisplayDTO
                                {
                                    SubRouteId = subroute.SubRouteId,
                                    RouteId = subroute.RouteId,
                                    FlightId = subroute.FlightId,
                                    ArrivalLocation = subroute.ArrivalLocation,
                                    DepartureLocation = subroute.DepartureLocation,
                                    ArrivalDateTime = subroute.ArrivalDateTime,
                                    DepartureDateTime = subroute.DepartureDateTime,
                                    SubFlightId = subroute.SubFlightId
                                })
                                .ToList();

                            if (routeSubroutes.Count == 0)
                            {
                                throw new Exception("Subroute flights are not available for route with id " + route.RouteId);
                            }

                            routesAndSubroutes.Add(route.RouteId, routeSubroutes);
                        }
                    }

                    flightsRoutesAndSubroutes.Add(flight.FlightId, routesAndSubroutes);
                }

                _logger.LogInformation("All flights, routes, and subroutes fetched successfully.");
                return flightsRoutesAndSubroutes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all flights, routes, and subroutes.");
                throw new FlightServiceException("Error Occurred While Getting All Flights, Routes, and Subroutes: " + ex.Message, ex);
            }
        }
        #endregion
        #region GetAllDirectFlights
        public async Task<Dictionary<int, List<RouteDTO>>> GetAllDirectFlights()
        {
            try
            {
                _logger.LogInformation("Fetching all direct flights...");
                var flights = await _flightRepository.GetAll();
                var allRoutes = await _routeRepository.GetAll();

                var flightsAndRoutes = new Dictionary<int, List<RouteDTO>>();

                foreach (var flight in flights)
                {
                    // Initialize the dictionary entry with an empty list
                    flightsAndRoutes[flight.FlightId] = new List<RouteDTO>();
                }

                foreach (var route in allRoutes)
                {
                    // Check if the route has no stops
                    if (route.NoOfStops == 0)
                    {
                        // Check if the route's flight ID is in the flights dictionary
                        if (flightsAndRoutes.ContainsKey(route.FlightId))
                        {
                            // Create a new RouteDTO object
                            var routeDto = new RouteDTO
                            {
                                FlightId = route.FlightId,
                                ArrivalLocation = route.ArrivalLocation,
                                ArrivalDateTime = route.ArrivalDateTime,
                                DepartureLocation = route.DepartureLocation,
                                DepartureDateTime = route.DepartureDateTime,
                                SeatsAvailable = route.SeatsAvailable,
                                NoOfStops = route.NoOfStops,
                                PricePerPerson = route.PricePerPerson
                            };

                            // Add the RouteDTO object to the corresponding flight's list
                            flightsAndRoutes[route.FlightId].Add(routeDto);
                        }
                    }
                }

                _logger.LogInformation("All direct flights fetched successfully.");
                return flightsAndRoutes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all direct flights.");
                throw new FlightServiceException("Error Occurred While Getting All Direct Flights: " + ex.Message, ex);
            }
        }
        #endregion



    }


}

