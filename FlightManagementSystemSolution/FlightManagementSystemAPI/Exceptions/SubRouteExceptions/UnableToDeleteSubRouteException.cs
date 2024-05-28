namespace FlightManagementSystemAPI.Exceptions.SubRouteExceptions
{
    public class UnableToDeleteSubRouteException : Exception
    {

        public UnableToDeleteSubRouteException(string? message) : base(message)
        {
        }

        public UnableToDeleteSubRouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
