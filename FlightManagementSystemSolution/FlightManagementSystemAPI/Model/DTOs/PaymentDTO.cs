using System.ComponentModel.DataAnnotations;

namespace FlightManagementSystemAPI.Model.DTOs
{
    public class PaymentDTO
    {
        public int BookingId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string PaymentMethod { get; set; }
      
    }
}
