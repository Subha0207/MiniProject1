namespace FlightManagementSystemAPI.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int Amount { get; set; }
        public string PaymentMode { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
