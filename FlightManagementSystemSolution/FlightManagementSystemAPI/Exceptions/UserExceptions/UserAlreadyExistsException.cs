namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string? message) : base(message)
        {

        }

    }
}

