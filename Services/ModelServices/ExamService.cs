using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class ExamsService {
    private readonly UniversidadContext _context;
    public ExamsService(UniversidadContext context) {
        _context = context;
    }
    /// <summary>
    /// Get all exams from the database as a list
    /// </summary>
    public async Task<List<Exam>> GetExams() {
        return await _context.Exams.ToListAsync();
    }
}
