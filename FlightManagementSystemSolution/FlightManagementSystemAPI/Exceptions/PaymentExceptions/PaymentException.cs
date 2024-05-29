namespace FlightManagementSystemAPI.Exceptions.PaymentExceptions
{
    public class PaymentException : Exception
    {
        public PaymentException(string? msg, Exception ex) : base(msg)
        {

        }
    }
}
