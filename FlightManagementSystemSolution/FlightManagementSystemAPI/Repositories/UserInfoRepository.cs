
using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FlightManagementSystemAPI.Repositories
{
    public class UserInfoRepository : IRepository<int, UserInfo>
    {
        private readonly FlightManagementContext _context;

        public  UserInfoRepository(FlightManagementContext context)
        {
            _context = context;
        }
        public async Task<UserInfo> Add(UserInfo item)
        {
            try
            {
                item.Status = "Disabled";
                var existingUserByEmail = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == item.Email);
                if (existingUserByEmail != null)
                {
                    throw new UserAlreadyExistsException("A user with the same email already exists.");
                }

                

                string emailRegex = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

                if (!Regex.IsMatch(item.Email, emailRegex))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (UserAlreadyExistsException ex)
            {
                throw new UserInfoException("Error: " + ex.Message, ex);
            }
            
            catch (ArgumentException ex)
            {
                throw new UserInfoException("Error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserInfoException("Error occurred while adding user info.", ex);
            }
        }


        public async  Task<UserInfo> Delete(int key)
        {
            try
            {
                var userInfo = await Get(key);
                _context.Remove(userInfo);
                await _context.SaveChangesAsync(true);
                return userInfo;
            }
            catch(UserNotFoundException ex)
            {
                throw new UserInfoException("Error occurred while deleting user info: " + ex.Message, ex);

            }
            catch(Exception ex)
            {
                throw new UserInfoException("Error while deleting user info",ex);
            }
        }

        public async Task<UserInfo> Get(int key)
        {
            try
            {
                var userInfo = await _context.UserInfos.FirstOrDefaultAsync(u => u.UserId == key);
                if (userInfo != null)
                {
                    return userInfo;
                }
                throw new UserNotFoundException("No user found with given user Id");
            }
            catch(UserNotFoundException ex)
            {
                throw new UserInfoException("Error occurred while getting user info: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserInfoException("Error occurred while getting user info.", ex);
            }
        }

        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            try
            {
                var userInfos = await _context.UserInfos.ToListAsync();
                if (userInfos.Count <= 0)
                {

                    throw new UserNotFoundException("No Users found");
                }
                return userInfos;
            }
            catch(UserNotFoundException ex)
            {


                throw new UserInfoException("Error occurred while getting user infos: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new UserInfoException("Error occurred while getting user infos.", ex);
            }

        }

        

        public async Task<UserInfo> Update(UserInfo item)
        {
            try
            {
                var userInfo = await Get(item.UserId);
                _context.Update(userInfo);
                await _context.SaveChangesAsync(true);
                return userInfo;

            }
            catch(UserNotFoundException ex)
            {
                throw new UserInfoException("Error occurred while updating user info. User info not found: " + ex.Message, ex);

            }
            catch (Exception ex)
            {
                throw new UserInfoException("Error occurred while updating user detail. User info not found: " , ex);

            }
        }
    }
}
