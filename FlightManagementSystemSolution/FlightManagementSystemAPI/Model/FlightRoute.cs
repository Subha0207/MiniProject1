using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagementSystemAPI.Model
{
    public class FlightRoute
    {
        public int RouteId { get; set; }
        
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public string DepartureLocation { get; set; }
        public DateTime DepartureDateTime { get; set; }
        
        public int SeatsAvailable { get; set; }
        public int NoOfStops { get; set; }
        public Flight Flight { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<SubRoute> SubRoutes { get; set; }
        public float PricePerPerson { get; set; }
    }
}
