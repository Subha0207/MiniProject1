namespace FlightManagementSystemAPI.Exceptions.BookingExceptions
{
    public class BookingNotFoundException:Exception
    {
        public BookingNotFoundException(string? message) : base(message)
        {
        }

        public BookingNotFoundException(string? msg, string message) : base(msg)
        {

        }

        public BookingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
