namespace FlightManagementSystemAPI.Exceptions.CancellationExceptions
{
    public class NoCancellationExistsException : Exception
    {
        public NoCancellationExistsException(string? msg) : base(msg)
        {

        }
    }
}
