using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models {
    /// <summary>
    /// Represents an exam in the database
    /// </summary>
    public class Exam {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Exam ID
        [Required]
        public int SId { get; set; } // Subject ID
        [Required]
        public DateTime Date { get; set; }

        public Exam(int sId, DateTime date) : this() {
            SId = sId;
            Date = date;
        }

        public Exam(){ }
    }
}
