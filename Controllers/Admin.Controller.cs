using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sysachad.Models;
using Sysachad.Services;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class AdminController : ControllerBase {
        private readonly StudentsService _studentsService;
        private readonly SubjectsService _subjectsService;

        public AdminController(StudentsService studentsService, SubjectsService subjectsService)
        {
            _studentsService = studentsService;
            _subjectsService = subjectsService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetData([FromQuery] string data)
        {
            switch (data)
            {
                case "students":
                    var students = await _studentsService.GetStudents();
                    return Ok(new { data = students });
                case "subjects":
                    var subjects = await _subjectsService.GetSubjects(); // no tocar, se mantiene
                    return Ok(new { data = subjects });
                case "classes":
                    var classes = await ClassesService.GetClasses(); // no tocar
                    return Ok(new { data = classes });
                default:
                    return BadRequest("Invalid data type");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserByID([FromRoute] int Id)
        {
            var users = await _studentsService.GetStudents();
            if (users.All(s => s.Id != Id))
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(users.FirstOrDefault(s => s.Id == Id));
        }

        public class AddStudentModel
        {
            public string Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public bool isAdmin { get; set; }
        }

        [HttpPost]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddUser([FromBody] AddStudentModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (new List<object> { student.Password }.Any(f => f == default))
            {
                return BadRequest("Faltan campos obligatorios");
            }

            var users = await _studentsService.GetStudents();

            Student newStudent = new Student(student.Name, student.Surname, student.Password, student.isAdmin);
            newStudent.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(student.Password, workFactor: 12);
            await _studentsService.AddStudent(newStudent);

            return Ok("Student added successfully, ID: " + newStudent.Id);
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateUser([FromBody] Student student)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (new List<object> { student.Name, student.Surname, student.Id, student.Password, student.IsAdmin }.Any(f => f == default)) {
                return BadRequest("Faltan campos obligatorios");
            }

            if (await _studentsService.SearchStudent(student.Id) == null) {
                return NotFound("Usuario no encontrado");
            } else {
                student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
                await _studentsService.UpdateStudent(student);
            }

            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var users = await _studentsService.GetStudents();
            if (users.All(s => s.Id != Id)) {
                return NotFound("Usuario no encontrado");
            }

            await _studentsService.DeleteStudent(Id);

            return Ok();
        }
    }
}
