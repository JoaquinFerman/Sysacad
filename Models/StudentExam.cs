using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models {
    /// <summary>
    /// Represents a registration of a student in an exam
    /// </summary>
    public class StudentExam {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EId { get; set; }
        [Required]
        public int StId { get; set; }

        public StudentExam(int eId, int stId) : this() {
            EId = eId;
            StId = stId;
        }

        public StudentExam(){ }
    }
}
