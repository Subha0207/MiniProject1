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
        public int NoOfPersons { get; set; }
        public float TotalAmount { get; set; }
        public ICollection<Cancellation> Cancellations { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
