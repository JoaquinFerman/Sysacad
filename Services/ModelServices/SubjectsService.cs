using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class SubjectsService {
    private readonly UniversidadContext _context;
    public SubjectsService(UniversidadContext context) {
        _context = context;
    }
    /// <summary>
    /// Gets all subjects from the database.
    /// </summary>
    /// <returns>
    /// A list of subjects
    /// </returns>
    public async Task<List<Subject>> GetSubjects() {
        return await _context.Subjects.ToListAsync();
    }
}
