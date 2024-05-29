namespace FlightManagementSystemAPI.Exceptions.RefundExceptions
{
    public class RefundException : Exception
    {
        public RefundException(string? message) : base(message)
        {
        }

        public RefundException(string? msg, string message) : base(msg)
        {

        }

        public RefundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
