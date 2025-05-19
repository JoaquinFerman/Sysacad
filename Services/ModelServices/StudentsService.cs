using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class StudentsService {
    private readonly UniversidadContext _context;

    public StudentsService(UniversidadContext context) {
        _context = context;
    }

    /// <summary>
    /// Get all students from the database
    /// </summary>
    /// <returns>
    /// Return all students from the database in a list
    /// </returns>
    public async Task<List<Student>> GetStudents() {
        return await _context.Students.ToListAsync();
    }

    /// <summary>
    /// Gets a student by ID
    /// </summary>
    /// <returns>
    /// Returns a single student with the given ID
    /// </returns>
    public async Task<Student?> SearchStudent(int id) {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Updates a student
    /// </summary>
    public async Task UpdateStudent(Student student) {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a new student to the database
    /// </summary>
    public async Task AddStudent(Student student) {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a student from the database
    /// </summary>
    public async Task<bool> DeleteStudent(int id) {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}
