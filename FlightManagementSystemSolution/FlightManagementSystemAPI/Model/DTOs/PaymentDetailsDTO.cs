namespace FlightManagementSystemAPI.Model.DTOs
{
    public class PaymentDetailsDTO
    {
        public int PaymentId { get; set; }
        public float Amount { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
