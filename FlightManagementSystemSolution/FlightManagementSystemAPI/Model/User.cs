﻿namespace FlightManagementSystemAPI.Model
{
    public class User
    {
        
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword{ get; set; }
        public string Role { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
