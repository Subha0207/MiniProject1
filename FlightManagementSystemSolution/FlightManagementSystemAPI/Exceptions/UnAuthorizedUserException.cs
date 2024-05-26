namespace FlightManagementSystemAPI.Exceptions
{
    public class UnAuthorizedUserException:Exception
    {
        public UnAuthorizedUserException(string? msg) : base(msg)
        {

        }
    }
}
