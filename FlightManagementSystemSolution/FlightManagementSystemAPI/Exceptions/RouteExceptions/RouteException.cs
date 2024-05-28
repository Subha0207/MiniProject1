namespace FlightManagementSystemAPI.Exceptions.RouteExceptions
{
    public class RouteException : Exception
    {

        public RouteException(string? msg, string message) : base(msg)
        {

        }

        public RouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
