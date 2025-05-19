using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class CorrelativesService {
    private readonly UniversidadContext _context;
    private readonly StudentSubjectsService _studentsSubjectsService;
    private readonly SubjectsService _subjectsService;
    private readonly ExamsService _examsService;
    private readonly StudentExamService _studentExamsService;
    public CorrelativesService(StudentSubjectsService studentsSubjectsService, SubjectsService subjectsService, ExamsService examsService, StudentExamService studentExamService, UniversidadContext context) {
        _studentsSubjectsService = studentsSubjectsService;
        _subjectsService = subjectsService;
        _examsService = examsService;
        _studentExamsService = studentExamService;
        _context = context;
    }
    /// <summary>
    /// Gets all correlatives from the database as a list
    /// </summary>
    public async Task<List<Correlative>> GetCorrelatives() {
        return await _context.Correlatives.ToListAsync();
    }

    /// <summary>
    /// Get all correlatives from a specific student for a certain process (Materia / Examen)
    /// </summary>
    /// <param name="forW">Which is the wanted process (Cursar / Rendir)</param>
    /// <param name="id">Student ID</param>
    /// <returns>
    /// A list with all subjects, indicating the conditions not met for the process in a list itself, or empty if all conditions are met
    /// </returns>
    public async Task<List<CorrelativeReturn>?> GetCorrelatives(string forW, int id){
        forW = forW == "cursar" ? "Cursar" : forW;
        forW = forW == "rendir" ? "Rendir" : forW;
        if(forW != "Cursar" && forW != "Rendir") {
            return null;
        }

        List<Correlative> correlatives = (await GetCorrelatives())
            .Where(c => c.TypeFor == forW)
            .ToList();

        List<StudentSubject> studentsSubjectsCoursed = (await _studentsSubjectsService.GetStudentsSubjects(id, "final")).ToList();
        List<StudentSubject> studentsSubjectsPassed = (await _studentsSubjectsService.GetStudentsSubjects(id, "passed")).ToList();
        
        List<Subject> subjects = await _subjectsService.GetSubjects();
        List<Exam> exams = await _examsService.GetExams();

        List<CorrelativeReturn> correlativesReturn = new List<CorrelativeReturn>();

        if(forW == "Cursar"){
            foreach(Subject subject in subjects) {
                List<string> conditions = new List<string>();

                foreach(Correlative correlative in correlatives) {
                    if(correlative.SId != subject.SId) continue;
                    if(correlative.Type == "Cursar" && !studentsSubjectsCoursed.Any(ss => ss.SubjectId == correlative.CId) && !studentsSubjectsPassed.Any(ss => ss.SubjectId == correlative.CId)) {
                        conditions.Add("Falta Cursar " + subjects.FirstOrDefault(s => s.SId == correlative.CId)?.Name);
                    }
                    if(correlative.Type == "Rendir" && !studentsSubjectsPassed.Any(ss => ss.SubjectId == correlative.CId)) {  
                        conditions.Add("Falta Rendir " + subjects.FirstOrDefault(s => s.SId == correlative.CId)?.Name);
                    }
                }

                if(!(await _studentsSubjectsService.GetStudentsSubjects(id)).Any(ss => ss.SubjectId == subject.SId)) {
                    correlativesReturn.Add(new CorrelativeReturn{
                        Year = subject.Year,
                        Name = subject.Name,
                        Condition = conditions.Count == 0 ? new List<string>() : conditions,
                        Plan = subject.Plan.Split("P")[1]
                    });
                }
            }
        } else {
            foreach(Exam exam in exams) {
                List<string> conditions = new List<string>();

                if(!studentsSubjectsCoursed.Any(ssc => ssc.SubjectId == exam.SId)) {
                    conditions.Add("Falta Cursar " + subjects.FirstOrDefault(s => s.SId == exam.SId)?.Name);
                }
                correlativesReturn.Add(new CorrelativeReturn{
                    Year = subjects.FirstOrDefault(s => s.SId == exam.SId)?.Year ?? 0,
                    Name = subjects.FirstOrDefault(s => s.SId == exam.SId)?.Name ?? "",
                    Condition = conditions.Count == 0 ? new List<string>() : conditions,
                    Plan = subjects.FirstOrDefault(s => s.SId == exam.SId)?.Plan.Split("P")[1] ?? ""
                });
            }
        }
        return correlativesReturn;
    }
}
