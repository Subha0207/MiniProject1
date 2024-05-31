using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IFlightService
    {

       /// <summary>
       /// Used to add Flight by admin
       /// </summary>
       /// <param name="flightDTO"></param>
       /// <returns></returns>
       public Task<FlightReturnDTO> AddFlight(FlightDTO flightDTO);
        /// <summary>
        /// Used to Update Flight details by admin
        /// </summary>
        /// <param name="FlightReturnDTO"></param>
        /// <returns></returns>
        public Task<FlightReturnDTO> UpdateFlight(FlightReturnDTO FlightReturnDTO);
        /// <summary>
        /// Used to delete flight by id by admin
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns></returns>
        public Task<FlightReturnDTO> DeleteFlight(int flightId);
     /// <summary>
     /// Used to get flight by flight id
     /// </summary>
     /// <param name="flightId"></param>
     /// <returns></returns>
     public Task<FlightReturnDTO> GetFlight(int flightId);
      /// <summary>
     /// Used to get all flights
      /// </summary>
      /// <returns></returns>
      public Task<List<FlightReturnDTO>> GetAllFlight();

        /// <summary>
        /// Used to get all the route and subroute of all flights
        /// </summary>
        /// <returns></returns>
        //  public Task<Dictionary<int, Dictionary<int, List<SubRoute>>>> GetAllFlightsRoutesAndSubroutes();
    
        public Task<Dictionary<int, Dictionary<int, List<SubRouteDisplayDTO>>>> GetAllFlightsRoutesAndSubroutes();

        /// <summary>
        /// Used to get all the Direct flights
        /// </summary>
        /// <returns></returns>
        public Task<Dictionary<int, List<RouteDTO>>> GetAllDirectFlights();
    }
}
