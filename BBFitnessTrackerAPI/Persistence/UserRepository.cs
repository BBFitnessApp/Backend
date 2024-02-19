using System.Collections.Generic;
using System.Linq;
using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //CRUD Tasks 
        public void Add(User user)
        {
             _dbContext.Users.Add(user);
        }

        public void Delete(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }

        //Other
        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return await _dbContext.Users.FirstAsync(u => u.Email == email && u.Password == password)!;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FirstAsync(u => u.Id == userId);
        }
    }
}