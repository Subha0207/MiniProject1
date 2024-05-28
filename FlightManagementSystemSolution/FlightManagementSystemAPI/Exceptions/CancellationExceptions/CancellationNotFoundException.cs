namespace FlightManagementSystemAPI.Exceptions.CancellationExceptions
{
    public class CancellationNotFoundException : Exception
    {

        public CancellationNotFoundException(string? msg) : base(msg)
        {

        }
    }
}
