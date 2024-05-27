namespace FlightManagementSystemAPI.Exceptions
{
    public class RouteServiceException : Exception
    {
        public RouteServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
