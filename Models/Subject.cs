using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models
{
    public class Subject {
        [Key]
        public int Id { get; set; } = 0;
        [Required]
        public int SId { get; set; }
        public string Name { get; set; } = "noName";

        public bool DurationBin { get; set; } = false; // 0 = Cuatrimestral, 1 = Anual
        [NotMapped]
        public string Duration => DurationBin ? "Anual" : "Cuatrimestral";

        public bool AttendanceBin { get; set; } = false; // 0 = Virtual, 1 = InPerson
        [NotMapped]
        public string Attendance => AttendanceBin ? "InPerson" : "Virtual";

        public int Year { get; set; }
        public SubjectsPlan PlanBin { get; set; } = 0; // 0 = Plan 2024, 1 = Plan 2003
        [NotMapped]
        public string Plan => PlanBin.ToString();

        public Subject(int sId, string name, string duration, string attendance, int year, SubjectsPlan plan) : this() {
            SId = sId;
            Name = name;
            DurationBin = duration == "Anual" ? true : false;
            AttendanceBin = attendance == "InPerson" ? true : false;
            Year = year;
            PlanBin = plan;
        }

        public Subject() {}
    }
}
