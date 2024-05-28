using FlightManagementSystemAPI.Model;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ITokenService
    {
        
            /// <summary>
            /// Used to Generate Token for JWT Authentication
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public string GenerateToken(User user);
        
    }
}
