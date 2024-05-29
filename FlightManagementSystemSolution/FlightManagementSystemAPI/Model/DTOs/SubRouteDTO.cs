namespace FlightManagementSystemAPI.Model.DTOs
{
    public class SubRouteDTO
    {
        public int FlightId { get; set; }
        public int RouteId { get; set; }
        public StopDTO[] Stops { get; set; }
    }

    public class StopDTO
    {
        public int SubFlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime DepartureDateTime { get; set; }
    }

}
