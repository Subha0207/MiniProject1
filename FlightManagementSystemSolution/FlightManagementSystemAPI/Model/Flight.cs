namespace FlightManagementSystemAPI.Model
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightName { get; set; }

        public int SeatCapacity { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public ICollection<SubRoute> SubRoutes { get; set; }

    }
}
