namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class FlightServiceException : Exception
    {
        public FlightServiceException(string? message) : base(message)
        {
        }

        public FlightServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
