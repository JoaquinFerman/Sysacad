using Sysachad.Models;

public class ClassesService {
    private readonly UniversidadContext _context;
    public ClassesService(UniversidadContext context) {
        _context = context;
    }
    public static async Task<List<Class>> GetClasses() {
        await using var context = new UniversidadContext();
        return context.Classes.ToList();
    }
}
