namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class UserException : Exception
    {

        string msg = string.Empty;

        public UserException(string? message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
            msg = message;
        }
        public override string Message => msg;

    }
}
