using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models {
    /// <summary>
    /// Represents a class in the university. It is not specific to a subject by the class ID itself, for that you need the subject ID
    /// </summary>
    public class Class {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SId { get; set; } // Subject
        [Required]
        public int CId { get; set; } //  Class itself
        public int Room { get; set; } // 0 = virtual
        public string Professor { get; set; }
        public int DaysBin { get; set; }
        [NotMapped]
        public List<string?> Days => DaysBin
            .ToString()
            .Distinct() // evita duplicados como 111
            .Select(d =>
            {
                return d switch
                {
                    '1' => "Lunes",
                    '2' => "Martes",
                    '3' => "Miercoles",
                    '4' => "Jueves",
                    '5' => "Viernes",
                    _ => null
                };
            })
            .Where(day => day != null)
            .ToList();
        public bool HoursBin { get; set; }
        [NotMapped]
        public int Hours => (int)Math.Pow(2, HoursBin ? 2 : 1);

        public Class(int sId, int cId, int room, string professor, int days, int hours) : this() {
            SId = sId;
            CId = cId;
            Room = room;
            Professor = professor;
            DaysBin = days;
            HoursBin = hours == 2;
        }
        public Class() {}
    }
}