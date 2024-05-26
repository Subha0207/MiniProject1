using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagementSystemAPI.Model
{
    public class FlightRoute
    {
        public int RouteId { get; set; }
        
        public int FlightId { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime ArrivalTime  { get; set; }
        public string DepartureLocation { get; set; }
        public DateTime DepartureDate { get; set; }

        public DateTime DepartureTime { get; set; }
        public int SeatsAvailable { get; set; }
        public int NoOfStops { get; set; }
        public Flight Flight { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<SubRoute> SubRoutes { get; set; }

    }
}
