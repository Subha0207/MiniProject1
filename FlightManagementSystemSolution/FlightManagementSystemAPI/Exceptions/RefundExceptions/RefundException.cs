namespace FlightManagementSystemAPI.Exceptions.RefundExceptions
{
    public class RefundException : Exception
    {

        public RefundException(string? msg, string message) : base(msg)
        {

        }

        public RefundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
