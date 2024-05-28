namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class UserInfoException : Exception
    {
        public UserInfoException(string? msg) : base(msg)
        {

        }

        string msg = string.Empty;
        public UserInfoException(string message, Exception innerException) : base(message, innerException)
        {
            msg = message;
        }
        public override string Message => msg;
    }
}
