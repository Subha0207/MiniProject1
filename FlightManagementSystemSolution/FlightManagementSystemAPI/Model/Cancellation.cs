namespace FlightManagementSystemAPI.Model
{
    public class Cancellation
    {
        public int CancellationId { get; set; }
        public int BookingId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
        public Booking Booking { get; set; }
        public ICollection<Refund> Refunds { get; set; }
    }
}
