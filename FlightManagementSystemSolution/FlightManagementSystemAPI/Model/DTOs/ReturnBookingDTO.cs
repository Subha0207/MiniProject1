namespace FlightManagementSystemAPI.Model.DTOs
{
    public class ReturnBookingDTO
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int RouteId { get; set; }
        public int NoOfPersons { get; set; }
        public float TotalAmount { get; set; }

    }
}
