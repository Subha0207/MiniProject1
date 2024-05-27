namespace FlightManagementSystemAPI.Exceptions
{
    public class RouteException: Exception
    {

        public RouteException(string? msg, string message) : base(msg)
        {

        }

        public RouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
