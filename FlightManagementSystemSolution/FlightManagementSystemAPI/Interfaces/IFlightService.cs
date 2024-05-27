using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IFlightService
    {

        public Task<FlightReturnDTO> AddFlight(FlightDTO flightDTO);
    }
}
