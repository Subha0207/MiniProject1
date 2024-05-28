namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class FlightException : Exception
    {
        public FlightException(string? msg, Exception ex) : base(msg)
        {

        }
    }
}
