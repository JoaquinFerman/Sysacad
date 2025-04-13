using Microsoft.EntityFrameworkCore;
using Sysachad.Models;
public class StudentsSubjectsService {
    private readonly UniversidadContext _context;
    public StudentsSubjectsService(UniversidadContext context) {
        _context = context;
    }
    public async Task<List<StudentsSubjects>> GetStudentsSubjects(int id, string? state = null) {
        List<StudentsSubjects> classes;
        switch (state) {
            case "oncourse":
                classes = await _context.StudentsSubjects
                    .Where(c => c.StudentId == id && c.StateBin == 0)
                    .ToListAsync();
                break;
            case "final":
                classes = await _context.StudentsSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.FINAL)
                    .ToListAsync();
                break;
            case "passed":
                classes = await _context.StudentsSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.PASSED)
                    .ToListAsync();
                break;
            case "available":
                classes = await _context.StudentsSubjects
                    .Where(c => c.StudentId == id && c.StateBin == StudentsSubjectsStates.ONCOURSE)
                    .ToListAsync();
                break;
            default:
                classes = await _context.StudentsSubjects
                    .Where(c => c.StudentId == id)
                    .ToListAsync();
                break;
        }
        return classes;
    }
}