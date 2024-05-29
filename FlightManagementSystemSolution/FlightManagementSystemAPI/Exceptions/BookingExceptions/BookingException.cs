namespace FlightManagementSystemAPI.Exceptions.BookingExceptions
{
    public class BookingException:Exception
    {
        public BookingException(string? message) : base(message)
        {
        }

        public BookingException(string? msg, string message) : base(msg)
        {

        }

        public BookingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
