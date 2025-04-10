using Sysachad.Models;

public class DivisionsService {
    private readonly UniversidadContext _context;
    public DivisionsService(UniversidadContext context) {
        _context = context;
    }
    public static async Task<List<Division>> GetDivisions() {
        await using var context = new UniversidadContext();
        return context.Divisions.ToList();
    }
}
