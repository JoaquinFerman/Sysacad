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
        public async Task<IActionResult> GetName() {
            var sId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Student student = (await StudentsService.GetStudents()).Find(s => s.SId == int.Parse(sId));
            return Ok( new { name = student.Name, surname = student.Surname } );
        }
    }
}