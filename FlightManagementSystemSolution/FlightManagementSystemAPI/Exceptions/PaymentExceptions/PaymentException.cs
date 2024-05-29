namespace FlightManagementSystemAPI.Exceptions.PaymentExceptions
{
    public class PaymentException : Exception
    {
        public PaymentException(string? message) : base(message)
        {
        }

        public PaymentException(string? msg, Exception ex) : base(msg)
        {

        }
    }
}
