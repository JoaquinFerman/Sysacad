using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sysachad.Models;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class AdminController : ControllerBase {

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetStudents() {
            var users = StudentsService.GetStudents();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserByID([FromRoute] int id) {
            var users = StudentsService.GetStudents();

            if (users.Any(s => s.Id == id) == false) {
                return NotFound("Usuario no encontrado");
            }

            return Ok(users.FirstOrDefault(s => s.Id == id));
        }

        public class AddStudentModel(){
            public int Id { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public bool isAdmin { get; set; }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AddUser([FromBody] AddStudentModel student) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (new List<Object> {student.Id, student.Password }.Any(f => f == default)){
                return BadRequest("Faltan campos obligatorios");
            } 
            var users = StudentsService.GetStudents();

            if (users.Any(s => s.Id == student.Id)) {
                return BadRequest("Id en uso");
            }
            Student newStudent = new Student(student.Name, student.Surname, student.Id, student.Password, student.isAdmin);
            newStudent.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(student.Password, workFactor: 12);
            StudentsService.AddStudent(newStudent);

            return CreatedAtAction(nameof(GetUserByID), new { newStudent.Id }, newStudent);
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateUser([FromBody] Student student) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (new List<Object> { student.Name, student.Surname, student.Id, student.Password, student.IsAdmin }.Any(f => f == default)){
                return BadRequest("Faltan campos obligatorios");
            }            
            if (StudentsService.SearchStudent(student.Id) != null) {
                return NotFound("Usuario no encontrado");
            } else {
                student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
                await StudentsService.UpdateStudent(student);
            }

            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id) {
            var users = StudentsService.GetStudents();
            if (users.Any(s => s.Id == id) == false) {
                return NotFound("Usuario no encontrado");
            }

            StudentsService.DeleteStudent(id);

            return Ok();
        }
    }
}
