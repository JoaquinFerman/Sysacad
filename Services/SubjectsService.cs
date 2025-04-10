using Sysachad.Models;

public class SubjectsService {
    private readonly UniversidadContext _context;
    public SubjectsService(UniversidadContext context) {
        _context = context;
    }
    public static async Task<List<Subject>> GetSubjects() {
        await using var context = new UniversidadContext();
        return context.Subjects.ToList();
    }
}
