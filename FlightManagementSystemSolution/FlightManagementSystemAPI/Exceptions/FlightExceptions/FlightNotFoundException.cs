namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException(string? msg) : base(msg)
        {

        }
    }
}
