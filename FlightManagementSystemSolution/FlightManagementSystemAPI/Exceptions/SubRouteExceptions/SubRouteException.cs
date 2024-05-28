namespace FlightManagementSystemAPI.Exceptions.SubRouteExceptions
{

    public class SubRouteException : Exception
    {

        public SubRouteException(string? msg, string message) : base(msg)
        {

        }

        public SubRouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
