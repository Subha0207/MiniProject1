namespace FlightManagementSystemAPI.Exceptions
{
    public class UnableToDeleteRouteException:Exception
    {
        public UnableToDeleteRouteException(string? message) : base(message)
        {
        }

        public UnableToDeleteRouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
