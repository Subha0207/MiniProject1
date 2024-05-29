namespace FlightManagementSystemAPI.Model.DTOs
{
    public class SubRouteReturnDTO
    {
        public int SubRouteId { get; set; }
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public int RouteId { get; set; }
    }
}
