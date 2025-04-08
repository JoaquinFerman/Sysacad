using MongoDB.Driver;
using Sysachad.Models;

public class StudentsService {
    private static readonly MongoClient client = MongoConnection.GetClient() != null ? MongoConnection.GetClient() : new MongoClient();
    private static readonly IMongoDatabase database = client.GetDatabase("Sysacad");
    private static readonly IMongoCollection<Student> usersDb = database.GetCollection<Student>("Students");

    public StudentsService() {
    }

    public static IMongoCollection<Student> GetStudentsDb() {
        return usersDb;
    }
    public static List<Student> GetStudents() {
        return usersDb.Find(_ => true).ToList();
    }

    public static Student SearchStudent(int id) {
        var users = usersDb.Find(u => u.Id == id).ToList();
        return users.FirstOrDefault();
    }
    public static async Task UpdateStudent(Student user) {
        var filtro = Builders<Student>.Filter.Eq(u => u.Id, user.Id);
        await usersDb.ReplaceOneAsync(filtro, user);
    }

    public static void AddStudent(Student user) {
        usersDb.InsertOneAsync(user);
    }

    public static bool DeleteStudent(int id) {
        var filtro = Builders<Student>.Filter.Eq(u => u.Id, id);
        var resultado = usersDb.DeleteOne(filtro);
        return resultado.DeletedCount > 0;
    }
}
