using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shoppingcart.Data;
using Shoppingcart.Models;
using System.Runtime.Serialization;

namespace Shoppingcart.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.FromSqlRaw("EXEC GetAllUsers").ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserById(int userId)
        {
            try
            {
                var param = new SqlParameter("@UserId", userId);

                var userdet = await _context.Users
                    .FromSqlRaw(@"exec GetUserById @UserId", param)
                    .ToListAsync();
              
                return userdet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred:Id not found");
                throw;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC InsertUser @Username, @Password, @Email, @Phonenumber, @Address",
                    new SqlParameter("@Username", user.Username),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@Phonenumber", user.PhoneNumber),
                    new SqlParameter("@Address", user.Address));


                return result > 0;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                      @"EXEC UpdateUser @UserId @Username, @Password, @Email, @Phonenumber, @Address",
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@Username", user.Username),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@Email", user.Email),
                    new SqlParameter("@Phonenumber", user.PhoneNumber),
                    new SqlParameter("@Address", user.Address));

                return user;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            int i = await Task.Run(() => _context.Database.ExecuteSqlInterpolatedAsync($"DeleteUser {userId}"));
            if (i == 1)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

      
    }

}
