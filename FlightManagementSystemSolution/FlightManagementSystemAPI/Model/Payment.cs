using System.Runtime.InteropServices;

namespace FlightManagementSystemAPI.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public float Amount { get; set; }
        public bool isSuccess { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public ICollection<Refund> Refunds { get; set; }
    }
}
