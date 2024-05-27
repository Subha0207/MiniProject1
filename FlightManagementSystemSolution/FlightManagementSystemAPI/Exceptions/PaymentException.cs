namespace FlightManagementSystemAPI.Exceptions
{
    public class PaymentException:Exception
    {

        public PaymentException(string? msg, string message) : base(msg)
        {

        }

        public PaymentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
