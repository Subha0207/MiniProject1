namespace FlightManagementSystemAPI.Exceptions.BookingExceptions
{
    public class InvalidNumberOfPersonException:Exception
    {
        public InvalidNumberOfPersonException(string? message) : base(message)
        {
        }

        public InvalidNumberOfPersonException(string? msg, string message) : base(msg)
        {

        }

        public InvalidNumberOfPersonException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
