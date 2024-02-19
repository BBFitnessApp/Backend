using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Contracts
{
    public interface IUserRepository
    {
     //   Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
