namespace FlightManagementSystemAPI.Model
{
    public class Cancellation
    {
        public int CancellationId { get; set; }
        public int BookingId { get; set; }
        public string Reason { get; set; }
        public int PaymentId { get; set; }
        public ICollection<Refund> Refunds { get; set; }
        public Payment Payment { get; set; }
        public Booking Booking { get; set; }
    }
}
