using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FlightManagementSystemAPI.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly FlightManagementContext _context;

        public UserRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<User> Add(User item)
        {
            try
            {
                var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == item.Email);
                if (existingUserByEmail != null)
                {
                    throw new UserAlreadyExistsException("A user with the same email already exists.");
                }

                if (item.Password != item.ConfirmPassword)
                {

                    throw new PasswordException("Password and confirm password must be the same");
                }
                
                string emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

                if (!Regex.IsMatch(item.Email, emailRegex))
                {
                    throw new EmailFormatException("Invalid email format.");
                }
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (UserAlreadyExistsException ex)
            {
                throw new UserException( ex.Message, ex);
            }
            catch(EmailFormatException ex)
            {
                throw new UserException( ex.Message, ex);
            }
            catch (PasswordException ex)
            {

                throw new UserException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserException("Error occurred while adding user info.", ex);
            }
        }

        public async Task<User> Delete(int key)
        {
            try
            {

                var user = await Get(key);
                _context.Remove(user);
                await _context.SaveChangesAsync(true);
                return user;
            }
            catch (UserNotFoundException ex)
            {
                throw new UserException("Error occurred while deleting user " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserException("Error occurred while deleting user.", ex);
            }
        }

        public async Task<User> Get(int key)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == key);
                if (user != null)
                    return user;
                throw new UserNotFoundException("No such user found");
             
            }
            catch (UserNotFoundException ex)
            {
                throw new UserException("Error occurred while getting user" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserException("Error occurred while getting user.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users.Count <= 0)
                {
                    throw new UserNotFoundException("There is not user Present");
                }
                return users;
            }
            catch (UserNotFoundException ex)
            {
                throw new UserException("Error occured while retrieving users" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserException("Error occurred while retrieving users.", ex);
            }
        }

        
        public async Task<User> Update(User item)
        {
            try
            {
                var user = await Get(item.UserId);
                _context.Update(user);
                await _context.SaveChangesAsync(true);
                return user;
            }
            catch (UserNotFoundException ex)
            {
                throw new UserException("Error occurred while updating user. User not found." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserException("Error occurred while updating user.", ex);
            }
        }
    }
}
