using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface ICalorieDataRepository
    {
        Task<int> GetCountAsync();
        Task<IEnumerable<CalorieData>> GetCalorieDataForUserAsync(int userId, DateTime startDate, DateTime endDate);

    }
}
