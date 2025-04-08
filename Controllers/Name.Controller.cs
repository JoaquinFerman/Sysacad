using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sysachad.Models;

namespace PrimeraWebAPI.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [Authorize]
    public class NameController : ControllerBase {
        [HttpGet]
        [Authorize]
        public IActionResult GetName() {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Student student = StudentsService.GetStudents().Find(s => s.Id == int.Parse(id));
            return Ok( new { name = student.Name, surname = student.Surname } );
        }
    }
}