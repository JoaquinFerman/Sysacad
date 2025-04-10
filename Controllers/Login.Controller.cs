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
        public LoginController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public class LoginModel {
            public int SId { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login) {
            Student? student = StudentsService.SearchStudent(login.SId);
            if (student != null && BCrypt.Net.BCrypt.EnhancedVerify(login.Password, student.Password)) {
                var token = _tokenService.GenerateToken(login.SId, student.IsAdmin);
                dynamic message = new ExpandoObject();
                message.Token = token;
                await StudentsService.UpdateStudent(student);
                return Ok( new {Token = token});
            }
            return Unauthorized("Invalid ID or password");
        }
    }
}
