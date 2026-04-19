using ESG.Api.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepo _userRepo;
        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepo.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: {ex.Message}");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepo.GetUserByEmail(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: {ex.Message}");
            }
        }
    }
}