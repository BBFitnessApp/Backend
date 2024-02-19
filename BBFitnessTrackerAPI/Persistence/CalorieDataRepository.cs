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

        //CRUD
        public void Add(CalorieData calorieData)
        {
            _dbContext.CalorieData.Add(calorieData);
        }

        public void Delete(CalorieData calorieData)
        {
            _dbContext.CalorieData.Remove(calorieData);
        }

        public void Update(CalorieData calorieData)
        {
            _dbContext.CalorieData.Update(calorieData);
        }
        public async Task<int> GetCountAsync()
        {
            return await _dbContext.CalorieData.CountAsync();
        }

        //other 
        public async Task<CalorieData> GetCalorieDataByUserAndDate(int userId, DateTime date)
        {
            return await _dbContext.CalorieData.FirstAsync(cd => cd.UserId == userId && cd.Datum.Date == date.Date);
        }

        //Abfrage zum Abrufen der täglichen Kalorieneinnahme eines Benutzers an einem bestimmten Datum:
        public Task<List<CalorieData>> GetCalorieDataByUserAndDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            return _dbContext.CalorieData.Where(cd => cd.UserId == userId && cd.Datum.Date >= startDate.Date && cd.Datum.Date <= endDate.Date).ToListAsync();
        }

        public async Task<CalorieData> GetCalorieDataByIdAsync(int calorieId)
        {
            return await _dbContext.CalorieData.FirstAsync(c => c.Id == calorieId); 
        }
    }
}