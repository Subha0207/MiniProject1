using FlightManagementSystemAPI.Model;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ITokenService
    {
        
            public string GenerateToken(User user);
        
    }
}
