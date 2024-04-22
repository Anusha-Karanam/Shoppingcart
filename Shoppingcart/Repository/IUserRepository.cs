using Shoppingcart.Models;

namespace Shoppingcart.Repository
{
    public interface IUserRepository
    {
       
        
        public Task<bool> AddUser(User user);
         public Task<User> UpdateUser(User user);
        public Task<bool> DeleteUser(int userId);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<IEnumerable<User>> GetUserById(int userId);
    }
}
