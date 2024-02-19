using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Web.ApiControllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(users);
        }

        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAndPassword(model.Email, model.Password);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(); // Save changes to the database
            return Ok(user);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
                return NotFound();

            _unitOfWork.UserRepository.Delete(user);
            return NoContent();
        }
    }
}
