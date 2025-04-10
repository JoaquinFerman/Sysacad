using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;

        public Student(string name, string surname, string password, bool isAdmin) : this() {
            Name = name;
            Surname = surname;
            Password = password;
            IsAdmin = isAdmin;
        }

        public Student(){
            Name = "NoName";
            Surname = "NoSurname";
            Password = "NoPassword";
        }
    }
}
