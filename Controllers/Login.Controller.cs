using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Sysachad.Services;
using Sysachad.Models;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly StudentsService _studentsService;

        public LoginController(TokenService tokenService, StudentsService studentsService)
        {
            _tokenService = tokenService;
            _studentsService = studentsService;
        }

        public class LoginModel
        {
            public int Id { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var student = await _studentsService.SearchStudent(login.Id);
            if (student != null && BCrypt.Net.BCrypt.EnhancedVerify(login.Password, student.Password))
            {
                var token = _tokenService.GenerateToken(login.Id, student.IsAdmin);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid ID or password");
        }
    }
}
