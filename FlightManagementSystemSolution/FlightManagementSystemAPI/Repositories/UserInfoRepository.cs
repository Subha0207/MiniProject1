using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlightManagementSystemAPI.Repositories
{
    public class UserInfoRepository : IRepository<int, UserInfo>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<UserInfoRepository> _logger;

        public UserInfoRepository(FlightManagementContext context, ILogger<UserInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region AddUserInfo
        public async Task<UserInfo> Add(UserInfo item)
        {
            try
            {
                _logger.LogInformation("Adding new user info.");
                item.Status = "Disabled";
                var existingUserByEmail = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == item.Email);
                if (existingUserByEmail != null)
                {
                    _logger.LogWarning("A user with the same email already exists.");
                    throw new UserAlreadyExistsException("A user with the same email already exists.");
                }

                string emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
                if (!Regex.IsMatch(item.Email, emailRegex))
                {
                    _logger.LogWarning("Invalid email format.");
                    throw new ArgumentException("Invalid email format.");
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User info added successfully.");
                return item;
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogError(ex, "Error: " + ex.Message);
                throw new UserInfoException("Error: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error: " + ex.Message);
                throw new UserInfoException("Error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user info.");
                throw new UserInfoException("Error occurred while adding user info.", ex);
            }
        }
        #endregion
        #region DeleteUserInfo
        public async Task<UserInfo> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting user info with key: {key}");
                var userInfo = await Get(key);
                _context.Remove(userInfo);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User info deleted successfully.");
                return userInfo;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user info: " + ex.Message);
                throw new UserInfoException("Error occurred while deleting user info: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user info.");
                throw new UserInfoException("Error while deleting user info", ex);
            }
        }
        #endregion
        #region GetUserInfo
        public async Task<UserInfo> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting user info with UserId: {key}");
                var userInfo = await _context.UserInfos.FirstOrDefaultAsync(u => u.UserId == key);
                if (userInfo != null)
                {
                    _logger.LogInformation("User info details fetched successfully.");
                    return userInfo;
                }
                _logger.LogWarning("No user found with given user Id.");
                throw new UserNotFoundException("No user found with given user Id");
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting user info: " + ex.Message);
                throw new UserInfoException("Error occurred while getting user info: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user info.");
                throw new UserInfoException("Error occurred while getting user info.", ex);
            }
        }
        #endregion
        #region GetAllUserInfo
        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all user info details.");
                var userInfos = await _context.UserInfos.ToListAsync();
                if (userInfos.Count == 0)
                {
                    _logger.LogWarning("No users found.");
                    throw new UserNotFoundException("No Users found");
                }
                _logger.LogInformation("User info details fetched successfully.");
                return userInfos;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting user infos: " + ex.Message);
                throw new UserInfoException("Error occurred while getting user infos: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user infos.");
                throw new UserInfoException("Error occurred while getting user infos.", ex);
            }
        }
        #endregion
        #region UpdateUserInfo
        public async Task<UserInfo> Update(UserInfo item)
        {
            try
            {
                _logger.LogInformation($"Updating user info with UserId: {item.UserId}");
                var userInfo = await Get(item.UserId);

                _context.Update(item);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User info updated successfully.");
                return userInfo;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while updating user info. User info not found: " + ex.Message);
                throw new UserInfoException("Error occurred while updating user info. User info not found: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user info.");
                throw new UserInfoException("Error occurred while updating user info.", ex);
            }
        }
        #endregion
    }
}
