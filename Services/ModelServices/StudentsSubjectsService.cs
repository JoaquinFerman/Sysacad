using Microsoft.EntityFrameworkCore;
using Sysachad.Models;
public class StudentSubjectsService {
    private readonly UniversidadContext _context;
    public StudentSubjectsService(UniversidadContext context) {
        _context = context;
    }
    /// <summary>
    /// Gets all the subjects of a student by its ID, and optionally filter by state
    /// </summary>
    /// <returns>
    /// A list of the StudentSubject's which the student is enrolled in and its state
    /// </returns>
    public async Task<List<StudentSubject>> GetStudentsSubjects(int id, string? state = null) {
        List<StudentSubject> classes;
        switch (state) {
            case "oncourse":
                classes = await _context.StudentSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.ONCOURSE)
                    .ToListAsync();
                break;
            case "final":
                classes = await _context.StudentSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.FINAL)
                    .ToListAsync();
                break;
            case "passed":
                classes = await _context.StudentSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.PASSED)
                    .ToListAsync();
                break;
            default:
                classes = await _context.StudentSubjects
                    .Where(c => c.StudentId == id)
                    .ToListAsync();
                break;
        }
        return classes;
    }

    /// <summary>
    /// Gets all the students of a class by its subject and class ID
    /// </summary>
    /// <returns><
    /// A list of the StudentSubject's in the given class
    /// /returns>
    public async Task<List<StudentSubject>> GetClassStudents(int subjectId, int classId) {
        List<StudentSubject> students = await _context.StudentSubjects
            .Where(c => c.SubjectId == subjectId && c.ClassId == classId)
            .ToListAsync();
        return students;
    }

    /// <summary>
    /// Registers a student in a class
    /// </summary>
    /// <returns>
    /// Confirmation of the registration
    /// </returns>
    public async Task<List<StudentSubject>> RegisterStudent(int id, int subjectId, int classId) {
        var studentSubject = new StudentSubject {
            StudentId = id,
            SubjectId = subjectId,
            ClassId = classId,
            StateBin = StudentsSubjectsStates.ONCOURSE
        };
        _context.StudentSubjects.Add(studentSubject);
        await _context.SaveChangesAsync();
        return await _context.StudentSubjects
            .Where(c => c.StudentId == id && c.SubjectId == subjectId && c.ClassId == classId)
            .ToListAsync();
    }
}