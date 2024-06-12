namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException(string? message) : base(message)
        {
        }

        public PaymentNotFoundException(string? msg, PaymentNotFoundException ex) : base(msg)
        {

        }
    }
}
