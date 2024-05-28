namespace FlightManagementSystemAPI.Exceptions.RouteExceptions
{
    public class UnableToDeleteRouteException : Exception
    {
        public UnableToDeleteRouteException(string? message) : base(message)
        {
        }

        public UnableToDeleteRouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
