namespace FlightManagementSystemAPI.Exceptions.PaymentExceptions
{
    public class EmptyPaymentMethodException:Exception
    {
        public EmptyPaymentMethodException()
        {
        }

        public EmptyPaymentMethodException(string? message) : base(message)
        {
        }

        public EmptyPaymentMethodException(string? msg, Exception ex) : base(msg)
        {

        }
    }
}
