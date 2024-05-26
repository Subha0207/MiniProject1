using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagementSystemAPI.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public Flight Flight { get; set; }
        public int FlightId { get; set; }
        public FlightRoute FlightRoute { get; set; }
        public int RouteId { get; set; }
       public DateTime BookingDateTime { get; set; }
        public Payment Payment { get; set; }
        public int PaymentId { get; set; }
        public ICollection<Cancellation> Cancellations { get; set; }
    }
}
