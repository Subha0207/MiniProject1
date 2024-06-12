namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException()
        {
        }

        public FlightNotFoundException(string? message) : base(message)
        {
        }

        public FlightNotFoundException(string? msg, FlightNotFoundException ex) : base(msg)
        {

        }
    }
}
