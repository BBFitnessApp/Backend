using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface ICalorieDataRepository
    {
        Task<int> GetCountAsync();
        Task<CalorieData> GetCalorieDataByUserAndDate(int userId, DateTime date);

        Task<List<CalorieData>> GetCalorieDataByUserAndDateRange(int userId, DateTime startDate, DateTime endDate);

        Task<CalorieData> GetCalorieDataByIdAsync(int calorieId);
        void Add(CalorieData calorieData);
        void Update(CalorieData calorieData);
        void Delete(CalorieData calorieData);
    }
}