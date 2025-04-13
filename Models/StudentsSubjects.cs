using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models
{
    public class StudentsSubjects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int ClassId { get; set; }
        public int? Grade { get; set; }
        public StudentsSubjectsStates StateBin { get; set; } // 0 = on course, 1 = final, 2 = passed
        [NotMapped]
        public string State => StateBin.ToString();

        public StudentsSubjects(int studentId, int subjectId, int classId, int? grade, StudentsSubjectsStates state){
            StudentId = studentId;
            SubjectId = subjectId;
            ClassId = classId;
            Grade = grade;
            StateBin = state;
        }

        public StudentsSubjects() { }
    }
}
