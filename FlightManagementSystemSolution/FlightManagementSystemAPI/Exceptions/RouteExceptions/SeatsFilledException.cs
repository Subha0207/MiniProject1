namespace FlightManagementSystemAPI.Exceptions.RouteExceptions
{
    public class SeatsFilledException:Exception
    {

        public SeatsFilledException(string? message) : base(message)
        {
        }

        public SeatsFilledException(string? msg, SeatsFilledException ex) : base(msg)
        {

        }
    }
}
