namespace FlightManagementSystemAPI.Model.DTOs
{
    public class RouteDTO
    {

        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime ArrivalTime { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime DepartureDate { get; set; }

        public DateTime DepartureTime { get; set; }
        public int SeatsAvailable { get; set; }
        public int NoOfStops { get; set; }
    }
}
