namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class UnAuthorizedUserException : Exception
    {
        public UnAuthorizedUserException(string? msg) : base(msg)
        {

        }
    }
}
