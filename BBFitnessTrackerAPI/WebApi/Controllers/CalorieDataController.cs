using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CalorieDataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalorieDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        


        [ProducesResponseType(typeof(List<CalorieData>), StatusCodes.Status200OK)]
        [HttpGet("{userId}/{date}")]
        public async Task<IActionResult> GetCalorieDataByUserAndDate(int userId, DateTime date)
        {
            var calorieData = await _unitOfWork.CalorieDataRepository.GetCalorieDataByUserAndDate(userId, date);
            if (calorieData == null)
                return NotFound();

            return Ok(calorieData);
        }

        [ProducesResponseType(typeof(List<CalorieData>), StatusCodes.Status200OK)]
        [HttpGet("{userId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetCalorieDataByUserAndDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            var calorieDataList = await _unitOfWork.CalorieDataRepository.GetCalorieDataByUserAndDateRange(userId, startDate, endDate);
            return Ok(calorieDataList);
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("count")]
        public async Task<IActionResult> GetCalorieDataCount()
        {
            var count = await _unitOfWork.CalorieDataRepository.GetCountAsync();
            return Ok(count);
        }

        [ProducesResponseType(typeof(CalorieData), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> AddCalorieData([FromBody] CalorieData calorieData)
        {
            _unitOfWork.CalorieDataRepository.Add(calorieData);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCalorieDataByUserAndDate), new { userId = calorieData.UserId, date = calorieData.Datum }, calorieData);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalorieData(int id)
        {
            var calorieData = await _unitOfWork.CalorieDataRepository.GetCalorieDataByIdAsync(id);
            if (calorieData == null)
                return NotFound();

            _unitOfWork.CalorieDataRepository.Delete(calorieData);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCalorieData(int id, [FromBody] CalorieData calorieData)
        {
            if (id != calorieData.Id)
                return BadRequest();

            _unitOfWork.CalorieDataRepository.Update(calorieData);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
