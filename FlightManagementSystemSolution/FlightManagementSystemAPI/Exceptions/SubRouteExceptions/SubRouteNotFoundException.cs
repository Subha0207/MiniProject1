namespace FlightManagementSystemAPI.Exceptions.SubRouteExceptions
{

    public class SubRouteNotFoundException : Exception
    {
        public SubRouteNotFoundException(string? message) : base(message)
        {
        }



        public SubRouteNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
