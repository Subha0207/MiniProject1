using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace FlightManagementSystemAPI.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly FlightManagementContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(FlightManagementContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Add(User item)
        {
            try
            {
                _logger.LogInformation("Adding new user.");
                var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == item.Email);
                if (existingUserByEmail != null)
                {
                    _logger.LogWarning("A user with the same email already exists.");
                    throw new UserAlreadyExistsException("A user with the same email already exists.");
                }

                if (item.Password != item.ConfirmPassword)
                {
                    _logger.LogWarning("Password and confirm password do not match.");
                    throw new PasswordException("Password and confirm password must be the same.");
                }

                string emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
                if (!Regex.IsMatch(item.Email, emailRegex))
                {
                    _logger.LogWarning("Invalid email format.");
                    throw new EmailFormatException("Invalid email format.");
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added successfully.");
                return item;
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new UserException(ex.Message, ex);
            }
            catch (EmailFormatException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new UserException(ex.Message, ex);
            }
            catch (PasswordException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new UserException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                throw new UserException("Error occurred while adding user.", ex);
            }
        }

        public async Task<User> Delete(int key)
        {
            try
            {
                _logger.LogInformation($"Deleting user with key: {key}");
                var user = await Get(key);
                _context.Remove(user);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User deleted successfully.");
                return user;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user: " + ex.Message);
                throw new UserException("Error occurred while deleting user: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                throw new UserException("Error occurred while deleting user.", ex);
            }
        }

        public async Task<User> Get(int key)
        {
            try
            {
                _logger.LogInformation($"Getting user with UserId: {key}");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == key);
                if (user != null)
                {
                    _logger.LogInformation("User details fetched successfully.");
                    return user;
                }
                _logger.LogWarning("No user found with given user Id.");
                throw new UserNotFoundException("No such user found.");
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while getting user: " + ex.Message);
                throw new UserException("Error occurred while getting user: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user.");
                throw new UserException("Error occurred while getting user.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all users.");
                var users = await _context.Users.ToListAsync();
                if (users.Count <= 0)
                {
                    _logger.LogWarning("No users found.");
                    throw new UserNotFoundException("There is no user present.");
                }
                _logger.LogInformation("Users fetched successfully.");
                return users;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users: " + ex.Message);
                throw new UserException("Error occurred while retrieving users: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                throw new UserException("Error occurred while retrieving users.", ex);
            }
        }

        public async Task<User> Update(User item)
        {
            try
            {
                _logger.LogInformation($"Updating user with UserId: {item.UserId}");
                var user = await Get(item.UserId);
                _context.Update(user);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("User updated successfully.");
                return user;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while updating user. User not found: " + ex.Message);
                throw new UserException("Error occurred while updating user. User not found: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                throw new UserException("Error occurred while updating user.", ex);
            }
        }
    }
}
