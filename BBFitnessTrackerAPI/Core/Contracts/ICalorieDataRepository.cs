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

        Task<double> ProteinIntakeByToday(int userId);
        Task<double> FetteIntakeByToday(int userId);
        Task<double> CalorieIntakeByDay(int userId, DateTime date);
        Task<double> KohlenhydrateByDay(int userId, DateTime date);

        Task<Dictionary<string, double>> IntakeByWeek(int userId, DateTime startDate);

        Task<Dictionary<string, double>> IntakeByMonth(int userId, int year, int month);

        Task<Dictionary<string, double>> IntakeByYear(int userId, int year);

        Task<Dictionary<int, List<CalorieData>>> GetCalorieDataGroupedByUserId(int userId);

        

        void Add(CalorieData calorieData);
        void Update(CalorieData calorieData);
        void Delete(CalorieData calorieData);
    }
}