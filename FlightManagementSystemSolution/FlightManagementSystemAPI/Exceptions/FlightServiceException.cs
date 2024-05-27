namespace FlightManagementSystemAPI.Exceptions
{
    public class FlightServiceException : Exception
    {
        public FlightServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
