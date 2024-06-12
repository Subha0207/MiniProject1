namespace FlightManagementSystemAPI.Exceptions.FlightExceptions
{
    public class InvalidSeatCapacityException : Exception
    {
        public InvalidSeatCapacityException(string message) : base(message) { }
    }

}
