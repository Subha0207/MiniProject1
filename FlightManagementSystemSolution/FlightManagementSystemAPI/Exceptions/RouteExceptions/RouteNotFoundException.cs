namespace FlightManagementSystemAPI.Exceptions.RouteExceptions
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(string? message) : base(message)
        {
        }

        public RouteNotFoundException(string? msg, RouteNotFoundException ex) : base(msg)
        {

        }
    }
}
