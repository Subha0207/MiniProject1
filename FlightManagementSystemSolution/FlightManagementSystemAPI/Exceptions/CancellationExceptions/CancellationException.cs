namespace FlightManagementSystemAPI.Exceptions.CancellationExceptions
{
    public class CancellationException : Exception
    {
        public CancellationException(string? message) : base(message)
        {
        }

        public CancellationException(string? msg, string message) : base(msg)
        {

        }

        public CancellationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
