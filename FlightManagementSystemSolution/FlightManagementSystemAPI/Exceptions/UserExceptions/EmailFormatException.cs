namespace FlightManagementSystemAPI.Exceptions.UserExceptions
{
    public class EmailFormatException : Exception
    {
        public EmailFormatException(string? message) : base(message)
        {
        }

        public EmailFormatException(string? msg, EmailFormatException ex) : base(msg)
        {

        }
    }
}
