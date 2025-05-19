using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sysachad.Models;
using System.Security.Claims;
using OneOf;
using OneOf.Types;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace Sysachad.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [Authorize]
    public class RegisterController : ControllerBase {
        private readonly StudentSubjectsService _studentSubjectsService;
        private readonly SubjectsService _subjectsService;
        private readonly ClassesService _classesService;
        private readonly CorrelativesService _correlativesService;
        private readonly StudentExamService _studentExamService;
        private readonly ExamsService _examsService;
        public RegisterController(StudentSubjectsService studentsSubjectsService, SubjectsService subjectsService, ClassesService classesService, CorrelativesService correlativesService, StudentExamService studentExamService, ExamsService examsService) {
            _examsService = examsService;
            _studentSubjectsService = studentsSubjectsService;
            _subjectsService = subjectsService;
            _classesService = classesService;
            _correlativesService = correlativesService;
            _studentExamService = studentExamService;
        }

        public class ClassesReturn {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Time { get; set; }
            public string Plan { get; set; }
            public int Year { get; set; }
        }

        public class RegisterRequest {
            public string ForW { get; set; }
            public int SubjectId { get; set; }
            public int ClassId { get; set; } = 0;
        }

        public async Task<List<Object>?> AvailableClasses(string forW, int SId, int id) {
            Subject subject = (await _subjectsService.GetSubjects()).FirstOrDefault(s => s.SId == SId);
            List<CorrelativeReturn> available = (await _correlativesService.GetCorrelatives(forW, id)).Where(c => c.Condition.Count() == 0).ToList();
            if(available.Any(a => a.Name == subject.Name)){
                if(forW == "Cursar"){
                    List<Class> allClasses = await _classesService.GetClasses();
                    List<Object> availableClasses = new List<Object>();

                    foreach (var c in allClasses.Where(c => c.SId == SId)) {
                        List<StudentSubject> studentSubjects = await _studentSubjectsService.GetClassStudents(SId, c.Id);
                        if (studentSubjects.Count() < 30) {
                            availableClasses.Add(c);
                        }
                    }
                    return availableClasses;
                }else if(forW == "Rendir"){
                    List<Exam> allExams = await _examsService.GetExams();
                    List<StudentExam> studentExams = await _studentExamService.GetExamsRegister();
                    List<Object> availableExams = allExams.Where(e => e.SId == SId && studentExams.Where(se => se.EId == e.Id).Count() < 30).Cast<object>().ToList();
                    return availableExams;
                }else{
                    return null;
                }
            } else {
                return null;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClasses([FromQuery]string forW, int SId) {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<Object>? result = await AvailableClasses(forW, SId, id);
            return Ok(new {result = result});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await AvailableClasses(request.ForW, request.SubjectId, id);
            if(result is null){
                return BadRequest("Error al inscribirse a esta materia");
            }else {
                if(request.ForW == "Cursar"){
                    if(result.OfType<Class>().Any(r => r.SId == request.SubjectId && r.CId == request.ClassId)){
                        await _studentSubjectsService.RegisterStudent(id, request.SubjectId, request.ClassId);
                        return Ok(new { retorno = "Registrado exitosamente a la materia " + request.SubjectId + " en la clase " + request.ClassId });
                    }
                } else if (request.ForW == "Rendir"){
                    List<StudentExam> registers = await _studentExamService.GetExamsRegister();
                    List<Exam> allExams = await _examsService.GetExams();
                    Exam studentExam = result.OfType<Exam>().FirstOrDefault(r => r.SId == request.SubjectId && r.Id == request.ClassId);
                    if(registers.Any(s => s.StId == id && s.EId == studentExam.Id)){
                        return BadRequest("Ya se encuentra registrado a un examen de esta materia");
                    }
                    if(studentExam != null){
                        Console.WriteLine(await _studentExamService.RegisterExam(id, studentExam));
                        return Ok(new { retorno = "Registrado exitosamente a rendir la materia " + request.SubjectId + " el dia " + studentExam.Date.Day + " a las " + studentExam.Date.Hour + ":" + studentExam.Date.Minute });
                    }
                }
            }
            return BadRequest("Error al inscribirse a esta materia");
        }
    }
}