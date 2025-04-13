using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sysachad.Models;
using System.Security.Claims;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [Authorize]
    public class ClassesController : ControllerBase {
        private readonly StudentsSubjectsService _studentsSubjectsService;
        private readonly SubjectsService _subjectsService;
        private readonly StudentsService _studentsService;
        private readonly ClassesService _classesService;
        public ClassesController(StudentsSubjectsService studentsSubjectsService, SubjectsService subjectsService, StudentsService studentsService, ClassesService classesService) {
            _studentsSubjectsService = studentsSubjectsService;
            _subjectsService = subjectsService;
            _studentsService = studentsService;
            _classesService = classesService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClasses([FromQuery] string? state = null) {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(state != null){
                return Ok( new { classes = await _studentsSubjectsService.GetStudentsSubjects(int.Parse(Id), state) } );
            } else {
                return Ok( new { subjects = await _subjectsService.GetSubjects() } );
            }
        }

        public class SubjectsReturn(){
            public int Year { get; set; }
            public string Name { get; set; }
            public string? State { get; set; }
            public int? Grade { get; set; }
            public int? ClassId { get; set; }
            public string Plan { get; set; }
        }

        [HttpGet("academic-state")]
        [Authorize]
        public async Task<IActionResult> GetAcademicState() {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<Subject> subjects = await _subjectsService.GetSubjects();
            List<StudentsSubjects> studentSubjects = await _studentsSubjectsService.GetStudentsSubjects(id);
            List<SubjectsReturn> subjectsReturn = new List<SubjectsReturn>();
            foreach(Subject subject in subjects){
                bool found = false;
                foreach(StudentsSubjects studentSubject in studentSubjects){
                    if(subject.SId == studentSubject.SubjectId){
                        subjectsReturn.Add(new SubjectsReturn{
                            Year = subject.Year,
                            Name = subject.Name,
                            State = studentSubject.State,
                            Grade = studentSubject.Grade,
                            ClassId = studentSubject.ClassId,
                            Plan = subject.Plan.Split("P")[1]
                        });
                        found = true;
                    }
                }
                if(!found){
                    subjectsReturn.Add(new SubjectsReturn{
                        Year = subject.Year,
                        Name = subject.Name,
                        State = null,
                        Grade = null,
                        ClassId = null,
                        Plan = subject.Plan.Split("P")[1]
                    });
                }
            }
            return Ok(new { subjects = subjectsReturn });
        }

        public class GradesReturn{
            public int SubjectId { get; set; }
            public string Name { get; set; }
            public int? Grade { get; set; }
            public string Plan { get; set; }
        }

        [HttpGet("grades")]
        [Authorize]
        public async Task<IActionResult> GetGrades() {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<StudentsSubjects> studentSubjects = await _studentsSubjectsService.GetStudentsSubjects(id, "passed");
            List<GradesReturn> grades = new List<GradesReturn>();
            foreach(StudentsSubjects studentSubject in studentSubjects){
                var subject = (await _subjectsService.GetSubjects()).FirstOrDefault(s => s.SId == studentSubject.SubjectId);
                if (subject != null) {
                    grades.Add(new GradesReturn{
                        SubjectId = studentSubject.SubjectId,
                        Name = subject.Name,
                        Grade = studentSubject.Grade,
                        Plan = subject.Plan.Split("P")[1]
                    });
                }
            }
            return Ok(new { grades = grades });
        }

        public class CourseReturn{
            public int Year { get; set; }
            public string Name { get; set; }
            public int? ClassId { get; set; }
            public int Room { get; set; }
            public string Professor { get; set; }
            public string Days { get; set; }
        }

        [HttpGet("courses")]
        [Authorize]
        public async Task<IActionResult> GetCourse() {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Student? student = await _studentsService.SearchStudent(id);
            List<CourseReturn> courses = new List<CourseReturn>();
            if (student == null) {
                return NotFound("Student not found");
            } else {
                foreach(StudentsSubjects sSubject in await _studentsSubjectsService.GetStudentsSubjects(id, "oncourse")){
                    Subject? subject = (await _subjectsService.GetSubjects()).FirstOrDefault(s => s.SId == sSubject.SubjectId);
                    Class? classs = (await _classesService.GetClasses()).FirstOrDefault(c => c.CId == sSubject.ClassId && c.SId == sSubject.SubjectId);
                    string time =
                        classs.CId.ToString().StartsWith("1") ? "8:30 - " + (8 + classs.Hours) + ":30" :
                        classs.CId.ToString().StartsWith("2") ? "13:30 - " + (13 + classs.Hours) + ":30" :
                        classs.CId.ToString().StartsWith("3") ? "18:30 - " + (18 + classs.Hours) + ":30" :
                        "Horario desconocido";

                    string days = string.Join(", ", classs.Days.Select(d => $"{d}: {time}"));

                    if (classs != null) {
                        if (sSubject != null && subject != null) {
                            courses.Add(new CourseReturn{
                                Year = subject.Year,
                                Name = subject.Name,
                                ClassId = classs.CId,
                                Room = classs.Room,
                                Professor = classs.Professor,
                                Days = days
                            });
                        }
                    }
                }
                return Ok( new {courses =  courses} );
            }
        }
    }
}