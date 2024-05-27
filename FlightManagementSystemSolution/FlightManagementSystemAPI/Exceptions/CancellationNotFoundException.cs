namespace FlightManagementSystemAPI.Exceptions
{
    public class CancellationNotFoundException : Exception
    {

        public CancellationNotFoundException(string? msg) : base(msg)
        {

        }
    }
}
