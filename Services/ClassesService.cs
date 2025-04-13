using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class ClassesService {
    private readonly UniversidadContext _context;
    public ClassesService(UniversidadContext context) {
        _context = context;
    }
    public async Task<List<Class>> GetClasses() {
        return await _context.Classes.ToListAsync();
    }
}
