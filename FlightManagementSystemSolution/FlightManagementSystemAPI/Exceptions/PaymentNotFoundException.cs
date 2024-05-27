namespace FlightManagementSystemAPI.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException(string? msg) : base(msg)
        {

        }
    }
}
