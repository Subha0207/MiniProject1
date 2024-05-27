namespace FlightManagementSystemAPI.Exceptions
{
    public class RefundNotFoundException : Exception
    {
        public RefundNotFoundException(string? message) : base(message)
        {
        }

        

        public RefundNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
