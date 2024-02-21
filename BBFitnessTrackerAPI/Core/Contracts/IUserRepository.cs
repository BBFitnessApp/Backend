using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPassword(string email, string password);

        Task<User> GetUserById(int userId);
        Task<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetAllAsync();
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        Task<int> GetCountAsync();
    }
}
