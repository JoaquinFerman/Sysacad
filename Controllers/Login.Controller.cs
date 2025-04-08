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
            public int Id { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login) {
            List<Student> students = StudentsService.GetStudents();
            if (students.Any(u => u.Id == login.Id && BCrypt.Net.BCrypt.EnhancedVerify(login.Password, u.Password))) {
                Student student = students.Find(u => u.Id == login.Id);
                var token = _tokenService.GenerateToken(login.Id, student.IsAdmin);
                dynamic message = new ExpandoObject();
                message.Token = token;
                StudentsService.UpdateStudent(student);
                return Ok( new {Token = token});
            }

            return Unauthorized("Invalid ID or password");
        }
    }
}
