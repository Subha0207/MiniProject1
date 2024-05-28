namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class UserNotActiveException : Exception
    {
        public UserNotActiveException(string? msg) : base(msg)
        {

        }
    }
}
