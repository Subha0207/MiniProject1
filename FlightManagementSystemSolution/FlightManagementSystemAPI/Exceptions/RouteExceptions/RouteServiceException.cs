﻿namespace FlightManagementSystemAPI.Exceptions.RouteExceptions
{
    public class RouteServiceException : Exception
    {
        public RouteServiceException(string? message) : base(message)
        {
        }

        public RouteServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
