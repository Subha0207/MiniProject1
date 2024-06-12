using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightManagementSystemAPI.Model
{
    public class UserInfo
    {
            [Key]
            public int UserId { get; set; }
            public string Email { get; set; }
            public byte[] Password { get; set; }
            public byte[] PasswordHashKey { get; set; }
            public string Status { get; set; }

        [ForeignKey("UserId")]
            public User User { get; set; }
     


    }
}
