using System.Threading.Tasks;
using Sysachad.Models;

public class StudentsService {
    private readonly UniversidadContext _context;
    public StudentsService(UniversidadContext context) {
        _context = context;
    }
    public static async Task<List<Student>> GetStudents() {
        await using var context = new UniversidadContext();
        return context.Students.ToList();
    }

    public static Student? SearchStudent(int sId) {
    using var context = new UniversidadContext();
    return context.Students.FirstOrDefault(s => s.SId == sId);
    }

    public static async Task UpdateStudent(Student student) {
        using var context = new UniversidadContext();
        context.Students.Update(student);
        await context.SaveChangesAsync();
    }

    public static async Task AddStudent(Student student) {
        using var context = new UniversidadContext();
        await context.AddAsync(student);
        await context.SaveChangesAsync();
    }

    public static async Task<bool> DeleteStudent(int sId) {
        using var context = new UniversidadContext();
        var student = context.Students.ToList().Find(s => s.SId == sId);
        var result = context.Remove<Student>(student);
        await context.SaveChangesAsync();
        return result != null;
    }
}
