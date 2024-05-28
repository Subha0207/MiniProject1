using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IUserService
    {
       /// <summary>
       /// Used for Role based Login  for User and admin and cannot access until account activated
       /// </summary>
       /// <param name="userLoginDTO"></param>
       /// <returns></returns>
       public Task<LoginReturnDTO> Login(LoginDTO userLoginDTO);
        /// <summary>
        /// Used to register by user and admin
        /// </summary>
        /// <param name="userRegisterDTO"></param>
        /// <returns></returns>
        public Task<RegisterReturnDTO> Register(RegisterDTO userRegisterDTO);

        /// <summary>
        /// User Activation by admin after registeration
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<int> UserActivation(int userId);
    }
}
