namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class EmailFormatException : Exception
    {
        public EmailFormatException(string? msg) : base(msg)
        {

        }
    }
}
