namespace FlightManagementSystemAPI.Model.DTOs
{
    public class BookingDTO
    {  
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public int RouteId { get; set; }
        public int NoOfPersons { get; set; }

    }
}
