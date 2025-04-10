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
        public async Task<IActionResult> GetData([FromQuery] string data) {
            switch (data) {
                case "students":
                    var students = await StudentsService.GetStudents();
                    return Ok( new { data = students });
                case "subjects":
                    var subjects = await SubjectsService.GetSubjects();
                    return Ok( new { data = subjects });
                case "divisions":
                    var divisions = await DivisionsService.GetDivisions();
                    return Ok( new { data = divisions });
                default:
                    return BadRequest("Invalid data type"); 
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserByID([FromRoute] int Id) {
            var users = await StudentsService.GetStudents();

            if (users.Any(s => s.Id == Id) == false) {
                return NotFound("Usuario no encontrado");
            }

            return Ok(users.FirstOrDefault(s => s.Id == Id));
        }

        public class AddStudentModel(){
            public string Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public bool isAdmin { get; set; }
        }

        [HttpPost]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddUser([FromBody] AddStudentModel student) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (new List<Object> { student.Password }.Any(f => f == default)){
                return BadRequest("Faltan campos obligatorios");
            } 
            var users = await StudentsService.GetStudents();

            Student newStudent = new Student(student.Name, student.Surname, student.Password, student.isAdmin);
            newStudent.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(student.Password, workFactor: 12);
            await StudentsService.AddStudent(newStudent);

            return Ok("Student added successfully, SID: " + newStudent.Id);
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
        public async Task<IActionResult> DeleteUser(int Id) {
            var users = await StudentsService.GetStudents();
            if (users.Any(s => s.Id == Id) == false) {
                return NotFound("Usuario no encontrado");
            }

            await StudentsService.DeleteStudent(Id);

            return Ok();
        }
    }
}
