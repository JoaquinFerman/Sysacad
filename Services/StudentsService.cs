using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

namespace Sysachad.Services {
    public class StudentsService {
        private readonly UniversidadContext _context;

        public StudentsService(UniversidadContext context) {
            _context = context;
        }

        public async Task<List<Student>> GetStudents() {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> SearchStudent(int id) {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateStudent(Student student) {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudent(Student student) {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteStudent(int id) {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
