using GrowthTracking.Repository.Base;
using GrowthTracking.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace GrowthTracking.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository()
        {

        }

        public async Task<User> GetUserAccount(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && u.IsDeleted == false);
        }

        public async Task<List<User>> GetDoctors()
        {
            return await _context.Users.Where(u => u.RoleCodeId == 2).ToListAsync();
        }
    }
}
