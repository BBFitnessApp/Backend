using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Persistence
{
    internal class CalorieDataRepository : ICalorieDataRepository
    {
        private ApplicationDbContext _dbContext;

        public CalorieDataRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IEnumerable<CalorieData>> GetCalorieDataForUserAsync(int userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountAsync()
        {
           
            return await _dbContext.CalorieData.CountAsync();
        }
    }
}