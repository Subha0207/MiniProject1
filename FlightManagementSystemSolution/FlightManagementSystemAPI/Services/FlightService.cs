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

        

        public async Task<Dictionary<int, Dictionary<int, List<SubRoute>>>> GetAllFlightsRoutesAndSubroutes()
        {
            var flights = await _flightRepository.GetAll();
            var allRoutes = await _routeRepository.GetAll();
            var allSubroutes = await _subrouteRepository.GetAll();

            var flightsRoutesAndSubroutes = new Dictionary<int, Dictionary<int, List<SubRoute>>>();

            foreach (var flight in flights)
            {
                var flightRoutes = new List<FlightRoute>();
                var routesAndSubroutes = new Dictionary<int, List<SubRoute>>();

                foreach (var route in allRoutes)
                {
                    if (route.FlightId == flight.FlightId)
                    {
                        flightRoutes.Add(route);
                        var routeSubroutes = new List<SubRoute>();

                        foreach (var subroute in allSubroutes)
                        {
                            if (subroute.RouteId == route.RouteId)
                            {
                                routeSubroutes.Add(subroute);
                            }
                        }

                        routesAndSubroutes.Add(route.RouteId, routeSubroutes);
                    }
                }

                flightsRoutesAndSubroutes.Add(flight.FlightId, routesAndSubroutes);
            }

            return flightsRoutesAndSubroutes;
        }


    }


}

