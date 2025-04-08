using MongoDB.Bson.Serialization.Attributes;
namespace Sysachad.Models
{
    [BsonIgnoreExtraElements]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;

        public Student(string name, string surname, int legajo, string password, bool isAdmin) : this() {
            Name = name;
            Surname = surname;
            Id = legajo;
            Password = password;
            IsAdmin = isAdmin;
        }

        public Student(){
            Id = 0;
            Name = "NoName";
            Surname = "NoSurname";
            Password = "NoPassword";
        }
    }
}
