namespace FlightManagementSystemAPI.Exceptions
{
    public class UnableToLoginException : Exception
    {
        public UnableToLoginException(string? msg):base(msg){
            }

        public UnableToLoginException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
