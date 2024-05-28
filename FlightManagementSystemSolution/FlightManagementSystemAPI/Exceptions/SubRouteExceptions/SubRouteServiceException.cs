namespace FlightManagementSystemAPI.Exceptions.SubRouteExceptions
{
    public class SubRouteServiceException : Exception
    {
        public SubRouteServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
