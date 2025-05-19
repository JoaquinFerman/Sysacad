using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sysachad.Models;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [Authorize]
    public class OptionsController : ControllerBase {
        private readonly StudentsService _studentsService;

        public OptionsController(StudentsService studentsService) {
            _studentsService = studentsService;
        }

        public class ChangePasswordModel {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel passwords) {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Student student = await _studentsService.SearchStudent(id);
            if (student != null && BCrypt.Net.BCrypt.EnhancedVerify(passwords.OldPassword, student.Password)) {
                student.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(passwords.NewPassword);
                await _studentsService.UpdateStudent(student);
                return Ok("Password changed successfully");
            }
            return Unauthorized("Invalid ID or password");
        }
    }
}
