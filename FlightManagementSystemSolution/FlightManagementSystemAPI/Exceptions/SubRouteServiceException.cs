namespace FlightManagementSystemAPI.Exceptions
{
    public class SubRouteServiceException : Exception
    {
        public SubRouteServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
