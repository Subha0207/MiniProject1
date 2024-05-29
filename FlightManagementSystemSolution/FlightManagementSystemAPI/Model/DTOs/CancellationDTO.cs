namespace FlightManagementSystemAPI.Model.DTOs
{
    public class CancellationDTO
    {
        
        public int BookingId { get; set; }
        public int PaymentId { get; set; }
        public string Reason { get; set; }
    }
}
