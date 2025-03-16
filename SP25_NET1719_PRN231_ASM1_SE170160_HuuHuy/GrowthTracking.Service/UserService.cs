using GrowthTracking.Repository;
using GrowthTracking.Repository.Models;

namespace GrowthTracking.Service
{
    public class UserService
    {
        private readonly UserRepository _repo;
        public UserService()
        {
            _repo = new UserRepository();

        }
        public async Task<User> Authenticate(string email, string password)
        {
            return await _repo.GetUserAccount(email, password);
        }

        public Task<List<User>> GetAllDoctors()
        {
            return _repo.GetDoctors();
        }
    }
}
