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

        Task<int> ProteinIntakeByToday(int userId);
        Task<int> FetteIntakeByToday(int userId);
        Task<int> CalorieIntakeByDay(int userId, DateTime date);
        Task<int> KohlenhydrateByDay(int userId, DateTime date);

        Task<Dictionary<string, int>> IntakeByWeek(int userId, DateTime startDate);

        Task<Dictionary<string, int>> IntakeByMonth(int userId, int year, int month);

        Task<Dictionary<string, int>> IntakeByYear(int userId, int year); 


        void Add(CalorieData calorieData);
        void Update(CalorieData calorieData);
        void Delete(CalorieData calorieData);
    }
}