namespace FlightManagementSystemAPI.Model.DTOs
{
    public class RouteDTO
    {

        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public string DepartureLocation { get; set; }
        public DateTime DepartureDateTime { get; set; }

        public int SeatsAvailable { get; set; }
        public int NoOfStops { get; set; }
        public float PricePerPerson { get; set; }
    }
}
