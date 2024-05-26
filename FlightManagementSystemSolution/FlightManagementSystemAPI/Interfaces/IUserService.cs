using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IUserService
    {
        public Task<LoginReturnDTO> Login(LoginDTO userLoginDTO);
        public Task<RegisterReturnDTO> Register(RegisterDTO userRegisterDTO);
    }
}
