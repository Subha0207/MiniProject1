namespace FlightManagementSystemAPI.Model.DTOs
{
    public class SubRouteDTO
    {
        public int RouteId { get; set; }
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public DateTime DepartureDateTime { get; set; }
    }
}
