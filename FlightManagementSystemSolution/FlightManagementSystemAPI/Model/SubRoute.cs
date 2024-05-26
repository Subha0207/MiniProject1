namespace FlightManagementSystemAPI.Model
{
    public class SubRoute
    {
        public int SubRouteId { get; set; }
        public int RouteId { get; set; }
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public DateTime DepartureDateTime { get; set; }

        public FlightRoute FlightRoute { get; set; }

        public Flight Flight { get; set; }
    }
}
