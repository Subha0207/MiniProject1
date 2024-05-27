namespace FlightManagementSystemAPI.Exceptions
{
    public class FlightNotFoundException:Exception
    {
        public FlightNotFoundException(string? msg) : base(msg)
        {

        }
    }
}
