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
        public async Task<int> ProteinIntakeByToday(int userId)
        {
            DateTime today = DateTime.Today;
            return await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Date == today)
                .SumAsync(cd => cd.Proteine);
        }

        public async Task<int> FetteIntakeByToday(int userId)
        {
            DateTime today = DateTime.Today;
            return await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Date == today)
                .SumAsync(cd => cd.Fette);
        }

        public async Task<int> CalorieIntakeByDay(int userId, DateTime date)
        {
            return await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Date == date.Date)
                .SumAsync(cd => cd.Kalorienaufnahme);
        }

        public async Task<int> KohlenhydrateByDay(int userId, DateTime date)
        {
            return await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Date == date.Date)
                .SumAsync(cd => cd.Kohlenhydrate);
        }

        public async Task<Dictionary<string, int>> IntakeByWeek(int userId, DateTime startDate)
        {
            DateTime endDate = startDate.AddDays(6);
            var weeklyData = await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Date >= startDate.Date && cd.Datum.Date <= endDate.Date)
                .ToListAsync();

            var intakeData = new Dictionary<string, int>();
            intakeData["Protein"] = weeklyData.Sum(cd => cd.Proteine);
            intakeData["Fette"] = weeklyData.Sum(cd => cd.Fette);
            intakeData["Calorien"] = weeklyData.Sum(cd => cd.Kalorienaufnahme);
            intakeData["Kohlenhydrate"] = weeklyData.Sum(cd => cd.Kohlenhydrate);

            return intakeData;
        }

        public async Task<Dictionary<string, int>> IntakeByMonth(int userId, int year, int month)
        {
            var monthlyData = await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Year == year && cd.Datum.Month == month)
                .ToListAsync();

            var intakeData = new Dictionary<string, int>();
            intakeData["Protein"] = monthlyData.Sum(cd => cd.Proteine);
            intakeData["Fette"] = monthlyData.Sum(cd => cd.Fette);
            intakeData["Calorien"] = monthlyData.Sum(cd => cd.Kalorienaufnahme);
            intakeData["Kohlenhydrate"] = monthlyData.Sum(cd => cd.Kohlenhydrate);

            return intakeData;
        }

        public async Task<Dictionary<string, int>> IntakeByYear(int userId, int year)
        {
            var yearlyData = await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId && cd.Datum.Year == year)
                .ToListAsync();

            var intakeData = new Dictionary<string, int>();
            intakeData["Protein"] = yearlyData.Sum(cd => cd.Proteine);
            intakeData["Fette"] = yearlyData.Sum(cd => cd.Fette);
            intakeData["Calorien"] = yearlyData.Sum(cd => cd.Kalorienaufnahme);
            intakeData["Kohlenhydrate"] = yearlyData.Sum(cd => cd.Kohlenhydrate);

            return intakeData;
        }

        public async Task<Dictionary<int, List<CalorieData>>> GetCalorieDataGroupedByUserId(int userId)
        {
            var calorieData = await _dbContext.CalorieData
                .Where(cd => cd.UserId == userId)
                .ToListAsync();

            var groupedData = calorieData
                .GroupBy(cd => cd.UserId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );

            return groupedData;
        }
    }
}