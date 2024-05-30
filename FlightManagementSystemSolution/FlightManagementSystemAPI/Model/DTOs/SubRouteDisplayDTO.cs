namespace FlightManagementSystemAPI.Model.DTOs
{
    public class SubRouteDisplayDTO
    {

        public int SubRouteId { get; set; }
        public int RouteId { get; set; }
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public DateTime DepartureDateTime { get; set; }

        public int SubFlightId { get; set; }
    }
}
