using Microsoft.EntityFrameworkCore;
using Sysachad.Models;
public class StudentExamService {
    private readonly UniversidadContext _context;
    public StudentExamService(UniversidadContext context) {
        _context = context;
    }

    /// <summary>
    /// Gets all student exams registered in the database
    /// </summary>
    /// <returns>
    /// A list of all student exams in the database
    /// </returns>
    public async Task<List<StudentExam>> GetExamsRegister() {
        return await _context.StudentExams.ToListAsync();
    }

    /// <summary>
    /// Registers a new exam for a student by both student and exam IDs
    /// </summary>
    /// <returns>
    /// A message indicating the result of the operation
    /// </returns>
    public async Task<string> RegisterExam(int stundentId, Exam exam){
        if (exam == null) {
            return "Exam not found";
        }
        var student = await _context.Students.FindAsync(stundentId);
        if (student == null) {
            return "Student not found";
        }
        var studentExam = new StudentExam {
            StId = stundentId,
            EId = exam.Id,
        };
        _context.StudentExams.Add(studentExam);
        await _context.SaveChangesAsync();
        return "Exam registered successfully";
    }
}