using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository<int, Flight> _flightRepository;

        public FlightService(IRepository<int, Flight> flightRepository)
        {
            _flightRepository = flightRepository;
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
                throw new FlightServiceException("Cannot add Flight this Moment some unwanted error occured :", ex);
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
    }
}
