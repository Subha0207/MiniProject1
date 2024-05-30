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

        public FlightService(IRepository<int, Flight> flightRepository, IRepository<int, FlightRoute> routeRepository, IRepository<int, SubRoute> subrouteRepository)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _subrouteRepository = subrouteRepository;
        }
         
        public async Task<FlightReturnDTO> AddFlight(FlightDTO flightDTO)
        {
            try
            {
                Flight flight = MapFlightDTOToFlight(flightDTO);
                Flight AddedFlight = await _flightRepository.Add(flight);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(AddedFlight);
                return flightReturnDTO;
            }
            catch (FlightException fr)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FlightServiceException("Error Occured,Unable to Add Flight", ex);
            }
        }

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

        public async Task<FlightReturnDTO> UpdateFlight(FlightReturnDTO FlightReturnDTO)
        {
            try
            {
                Flight flight = MapFlightReturnDTOWithFlight(FlightReturnDTO);
                Flight UpdatedFlight = await _flightRepository.Update(flight);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(UpdatedFlight);
                return flightReturnDTO;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FlightServiceException("Error Occured,Unable to Update Flight" + e.Message, e);
            }
        }

        public async Task<FlightReturnDTO> DeleteFlight(int flightId)
        {
            try
            {
                Flight flight = await _flightRepository.Delete(flightId);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(flight);
                return flightReturnDTO;
            }
            catch (FlightException)
            {
                throw;
            }
            catch (FlightServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FlightServiceException("Error occurred while deleting the flight: " + ex.Message, ex);
            }
        }

        public async Task<FlightReturnDTO> GetFlight(int flightId)
        {
            try
            {
                Flight flight = await _flightRepository.Get(flightId);
                FlightReturnDTO flightReturnDTO = MapFlightToFlightReturnDTO(flight);
                return flightReturnDTO;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FlightServiceException("Error Occured,Unable to Get Flight" + e.Message, e);
            }
        }

        public async Task<List<FlightReturnDTO>> GetAllFlight()
        {
            try
            {
                var flights = await _flightRepository.GetAll();
                List<FlightReturnDTO> flightReturnDTOs = new List<FlightReturnDTO>();
                foreach (Flight flight in flights)
                {
                    flightReturnDTOs.Add(MapFlightToFlightReturnDTO(flight));
                }
                return flightReturnDTOs;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FlightServiceException("Error Occured While Getting All the Flight" + ex.Message, ex);
            }
        }

        private Flight MapFlightReturnDTOWithFlight(FlightReturnDTO flightReturnDTO)
        {
            Flight flight = new Flight();
            flight.FlightId = flightReturnDTO.FlightId;
            flight.FlightName = flightReturnDTO.FlightName;
            flight.SeatCapacity= flightReturnDTO.SeatCapacity;
            return flight;
        }

        public async Task<Dictionary<int, Dictionary<int, List<SubRouteDisplayDTO>>>> GetAllFlightsRoutesAndSubroutes()
        {
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

            return flightsRoutesAndSubroutes;
        }




        public async Task<Dictionary<int, List<RouteDTO>>> GetAllDirectFlights()
        {
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

            return flightsAndRoutes;
        }

   


    }


}

