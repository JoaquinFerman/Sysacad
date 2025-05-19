using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class UniversidadContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<Correlative> Correlatives { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<StudentExam> StudentExams { get; set; }
    
    public UniversidadContext() 
    : base(new DbContextOptionsBuilder<UniversidadContext>()
           .UseSqlServer("Server=localhost,1433;Database=UniversidadDB;User Id=sa;Password=" + Environment.GetEnvironmentVariable("SqlSAccess") +";TrustServerCertificate=True;")
           .Options) { }
}