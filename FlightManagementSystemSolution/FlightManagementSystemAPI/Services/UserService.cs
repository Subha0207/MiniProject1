using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<int, User> userRepo, IRepository<int, UserInfo> userInfoRepo, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _userInfoRepo = userInfoRepo;
            _tokenService = tokenService;
            _logger = logger;
        }

        #region MapperMethods
        private User GenerateUser(RegisterDTO registerDTO)
        {
            _logger.LogInformation("GenerateUser method called with parameters: {RegisterDTO}", registerDTO);
            User user = new User();
            user.Name = registerDTO.Name;
            user.Email = registerDTO.Email;
            user.ConfirmPassword = registerDTO.ConfirmPassword;
            user.Password = registerDTO.Password;
            user.Role = registerDTO.Role;
            _logger.LogInformation("User generated: {User}", user);
            return user;
        }

        private UserInfo MapRegisterDTOToUserInfo(RegisterDTO registerDTO)
        {
            _logger.LogInformation("MapRegisterDTOToUserInfo method called with parameters: {RegisterDTO}", registerDTO);
            UserInfo userInfo = new UserInfo();
            userInfo.Status = "Disabled";
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            userInfo.PasswordHashKey = hMACSHA512.Key;
            userInfo.Email = registerDTO.Email;
            userInfo.Password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            _logger.LogInformation("UserInfo mapped from RegisterDTO: {UserInfo}", userInfo);
            return userInfo;
        }

        private RegisterReturnDTO MapUserToReturnDTO(User user)
        {
            _logger.LogInformation("MapUserToReturnDTO method called with parameters: {User}", user);
            RegisterReturnDTO returnDTO = new RegisterReturnDTO();
            returnDTO.UserId = user.UserId;
            returnDTO.Email = user.Email;
            returnDTO.Name = user.Name;
            returnDTO.Role = user.Role;
            _logger.LogInformation("RegisterReturnDTO mapped from User: {RegisterReturnDTO}", returnDTO);
            return returnDTO;
        }

        private LoginReturnDTO MapUserToLoginReturnDTO(User user)
        {
            _logger.LogInformation("MapUserToLoginReturnDTO method called with parameters: {User}", user);
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.UserId = user.UserId;
            returnDTO.Role = user.Role;
            returnDTO.Email = user.Email;
            returnDTO.Token = _tokenService.GenerateToken(user);
            _logger.LogInformation("LoginReturnDTO mapped from User: {LoginReturnDTO}", returnDTO);
            return returnDTO;
        }
        #endregion   
        #region Validations
        public bool IsValidEmail(string email)
        {
            _logger.LogInformation("IsValidEmail method called with parameters: {Email}", email);
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            Regex regex = new Regex(pattern);
            bool isValid = regex.IsMatch(email);
            _logger.LogInformation("Email validation result for {Email}: {IsValid}", email, isValid);
            return isValid;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            _logger.LogInformation("ComparePassword method called");
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    _logger.LogInformation("Password comparison result: {Result}", false);
                    return false;
                }
            }
            _logger.LogInformation("Password comparison result: {Result}", true);
            return true;
        }
        #endregion
        #region Register
        public async Task<RegisterReturnDTO> Register(RegisterDTO registerDTO)
        {
            _logger.LogInformation("Register method called with parameters: {RegisterDTO}", registerDTO);
            try
            {
                User user1 = GenerateUser(registerDTO);
                UserInfo userInfo1 = MapRegisterDTOToUserInfo(registerDTO);
                User user = await _userRepo.Add(user1);
                userInfo1.UserId = user.UserId;
                UserInfo userInfo = await _userInfoRepo.Add(userInfo1);
                RegisterReturnDTO registerReturnDTO = MapUserToReturnDTO(user);
                _logger.LogInformation("User registered successfully: {RegisterReturnDTO}", registerReturnDTO);
                return registerReturnDTO;
            }
            catch (UserException ex)
            {
                _logger.LogError(ex, "UserException occurred while registering user: {RegisterDTO}", registerDTO);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while registering user: {RegisterDTO}", registerDTO);
                throw new UnableToRegisterException("An error occurred during registration: " + ex.Message);
            }
        }
        #endregion
        #region Login
        public async Task<LoginReturnDTO> Login(LoginDTO loginDTO)
        {
            _logger.LogInformation("Login method called with parameters: {LoginDTO}", loginDTO);
            try
            {
                var userDb = await _userInfoRepo.Get(loginDTO.UserId);
                HMACSHA512 hMACSHA = new HMACSHA512(userDb.PasswordHashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, userDb.Password);
                if (isPasswordSame)
                {
                    var user = await _userRepo.Get(loginDTO.UserId);

                    if (userDb.Status == "active")
                    {
                        LoginReturnDTO loginReturnDTO = MapUserToLoginReturnDTO(user);
                        _logger.LogInformation("User logged in successfully: {LoginReturnDTO}", loginReturnDTO);
                        return loginReturnDTO;
                    }
                    throw new UserNotActiveException("Your account is not activated");
                }
                throw new UnAuthorizedUserException("Invalid UserName or Password");
            }
            catch (UserNotActiveException ex)
            {
                _logger.LogError(ex, "UserNotActiveException occurred while logging in user: {LoginDTO}", loginDTO);
                throw;
            }
            catch (UnAuthorizedUserException ex)
            {
                _logger.LogError(ex, "UnAuthorizedUserException occurred while logging in user: {LoginDTO}", loginDTO);
                throw;
            }
            catch (UserInfoException ex)
            {
                _logger.LogError(ex, "UserInfoException occurred while logging in user: {LoginDTO}", loginDTO);
                throw;
            }
            catch (UserException ex)
            {
                _logger.LogError(ex, "UserException occurred while logging in user: {LoginDTO}", loginDTO);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while logging in user: {LoginDTO}", loginDTO);
                throw new UnableToLoginException("Not Able to Register User at this moment", ex);
            }
        }
        #endregion
        #region UserActivation
        public async Task<int> UserActivation(int userId)
        {
            _logger.LogInformation("UserActivation method called with parameters: {UserId}", userId);
            try
            {
                var user = await _userInfoRepo.Get(userId);
                if (user != null)
                {
                    user.Status = "active";
                    var updatedUser = await _userInfoRepo.Update(user);
                    if (updatedUser != null)
                    {
                        _logger.LogInformation("User {UserId} activated successfully.", userId);
                        return updatedUser.UserId;
                    }
                    throw new Exception("Cannot get the updated user details");
                }
                throw new Exception("Cannot get the user");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while activating user: {UserId}", userId);
                throw;
            }
        }
        #endregion
      
 
    }
}
