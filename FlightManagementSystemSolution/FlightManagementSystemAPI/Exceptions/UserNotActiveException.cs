namespace FlightManagementSystemAPI.Exceptions
{
    public class UserNotActiveException:Exception
    {
        public UserNotActiveException(string? msg) : base(msg)
        {

        }
    }
}
