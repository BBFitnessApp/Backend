using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("AddCalorieData")]
        public async Task<IActionResult> AddCalorieData([FromBody] CalorieData calorieData)
        {
            _unitOfWork.CalorieDataRepository.Add(calorieData);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCalorieDataByUserAndDate), new { userId = calorieData.UserId, date = calorieData.Datum }, calorieData);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("DeleteCalorieData/{id}")]
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
        [HttpPut("UpdateCalorieData/{id}")]
        public async Task<IActionResult> UpdateCalorieData(int id, [FromBody] CalorieData calorieData)
        {
            if (id != calorieData.Id)
                return BadRequest();

            _unitOfWork.CalorieDataRepository.Update(calorieData);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("ProteinIntakeByToday/{email}")]
        public async Task<IActionResult> ProteinIntakeByToday(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var proteinIntake = await _unitOfWork.CalorieDataRepository.ProteinIntakeByToday(user.Id);
            return Ok(proteinIntake);
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("FetteIntakeByToday/{email}")]
        public async Task<IActionResult> FetteIntakeByToday(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var fetteIntake = await _unitOfWork.CalorieDataRepository.FetteIntakeByToday(user.Id);
            return Ok(fetteIntake);
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("CalorieIntakeByToday/{email}/{date}")]
        public async Task<IActionResult> CalorieIntakeByDay(string email, DateTime date)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var calorieIntake = await _unitOfWork.CalorieDataRepository.CalorieIntakeByDay(user.Id, date);
            return Ok(calorieIntake);
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("KohlenhydrateByDay/{email}/{date}")]
        public async Task<IActionResult> KohlenhydrateByDay(string email, DateTime date)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var kohlenhydrateIntake = await _unitOfWork.CalorieDataRepository.KohlenhydrateByDay(user.Id, date);
            return Ok(kohlenhydrateIntake);
        }

        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        [HttpGet("IntakeByWeek/{email}/{startDate}")]
        public async Task<IActionResult> IntakeByWeek(string email, DateTime startDate)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var intakeData = await _unitOfWork.CalorieDataRepository.IntakeByWeek(user.Id, startDate);
            return Ok(intakeData);
        }

        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        [HttpGet("IntakeByMonth/{email}/{year}/{month}")]
        public async Task<IActionResult> IntakeByMonth(string email, int year, int month)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var intakeData = await _unitOfWork.CalorieDataRepository.IntakeByMonth(user.Id, year, month);
            return Ok(intakeData);
        }

        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        [HttpGet("IntakeByYear/{email}/{year}")]
        public async Task<IActionResult> IntakeByYear(string email, int year)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            var intakeData = await _unitOfWork.CalorieDataRepository.IntakeByYear(user.Id, year);
            return Ok(intakeData);
        }


        [HttpGet("GetCalorieDataGroupedByUserId/calorieData/{userId}")]
        public async Task<IActionResult> GetCalorieDataGroupedByUserId(int userId)
        {
            var groupedData = await _unitOfWork.CalorieDataRepository.GetCalorieDataGroupedByUserId(userId);
            return Ok(groupedData);
        }
    }
}