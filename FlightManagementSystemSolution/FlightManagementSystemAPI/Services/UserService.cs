using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FlightManagementSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepo;
        private readonly IRepository<int, UserInfo> _userInfoRepo;
        private readonly ITokenService _tokenService;

        public UserService(IRepository<int, User> userRepo, IRepository<int, UserInfo> userInfoRepo,ITokenService tokenService)
        {
            _userRepo = userRepo;
            _userInfoRepo = userInfoRepo;
            _tokenService = tokenService;
        }


        public async Task<RegisterReturnDTO> Register(RegisterDTO registerDTO)
        {
            
            try
            {
                User user1 = GenerateUser(registerDTO);
                UserInfo userInfo1 = MapRegisterDTOToUserInfo(registerDTO);
                User user = await _userRepo.Add(user1);
                userInfo1.UserId = user.UserId;
                UserInfo userInfo = await _userInfoRepo.Add(userInfo1);
                RegisterReturnDTO registerReturnDTO = MapUserToReturnDTO(user);
                return registerReturnDTO;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnableToRegisterException("An error occurred during registration: " + ex.Message);
            }
        }


        public bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<LoginReturnDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                var userDb = await _userInfoRepo.Get(loginDTO.UserId);
                HMACSHA512 hMACSHA = new HMACSHA512(userDb.PasswordHashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, userDb.Password);
                if (isPasswordSame)
                {
                    var user = await _userRepo.Get(loginDTO.UserId);
                    LoginReturnDTO loginReturnDTO = MapUserToLoginReturnDTO(user);
                    return loginReturnDTO;
                }
                throw new UnAuthorizedUserException("Invalid UserName or Password");
            }
            catch (UnAuthorizedUserException)
            {
                throw;
            }
            catch (UserInfoException)
            {
                throw;
            }
            catch (UserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnableToLoginException("Not Able to Register User at this moment", ex);
            }
        }

        private User GenerateUser(RegisterDTO registerDTO)
        {
            User user = new User();
            user.Name = registerDTO.Name;
            user.Email = registerDTO.Email;
            user.ConfirmPassword = registerDTO.ConfirmPassword;
            user.Password = registerDTO.Password;
            return user;
        }
        private UserInfo MapRegisterDTOToUserInfo(RegisterDTO registerDTO)
        {
            UserInfo userInfo = new UserInfo();
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            userInfo.PasswordHashKey = hMACSHA512.Key;
            userInfo.Email = registerDTO.Email;
            userInfo.Password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            return userInfo;
        }

        private RegisterReturnDTO MapUserToReturnDTO(User user)
        {
            RegisterReturnDTO returnDTO = new RegisterReturnDTO();
            returnDTO.UserId = user.UserId;
            returnDTO.Email = user.Email;
            returnDTO.Name = user.Name;
           
            return returnDTO;
        }
        private LoginReturnDTO MapUserToLoginReturnDTO(User user)
        {
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.UserId = user.UserId;
            returnDTO.Role = user.Role;
            returnDTO.Email = user.Email;
            returnDTO.Token = _tokenService.GenerateToken(user);
            return returnDTO;
        }

    }
}
