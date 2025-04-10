using Microsoft.EntityFrameworkCore;
using Sysachad.Models;

public class UniversidadContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Division> Divisions { get; set; }
    
    public UniversidadContext() 
    : base(new DbContextOptionsBuilder<UniversidadContext>()
           .UseSqlServer("Server=localhost,1433;Database=UniversidadDB;User Id=sa;Password=F3rm4n2025!;TrustServerCertificate=True;")
           .Options) { }
}