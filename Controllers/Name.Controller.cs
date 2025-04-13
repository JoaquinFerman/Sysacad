using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sysachad.Models;
using Sysachad.Services;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [Authorize]
    public class NameController : ControllerBase
    {
        private readonly StudentsService _studentsService;

        public NameController(StudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetName() {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var students = await _studentsService.GetStudents();
            Student student = students.Find(s => s.Id == int.Parse(Id));
            return Ok(new { name = student.Name, surname = student.Surname });
        }
    }
}
