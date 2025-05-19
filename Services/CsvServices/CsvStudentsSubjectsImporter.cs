using Sysachad.Models;

public class CsvStudentsSubjectsImporter {
    private readonly UniversidadContext _context;

    public CsvStudentsSubjectsImporter(UniversidadContext context) {
        _context = context;
    }

    public async Task ImportFromCsv(string filePath) {
        if (_context.StudentSubjects.Any()) {
            Console.WriteLine("La tabla StudentsSubjects ya tiene datos. No se importará nada.");
            return;
        }
        
        var lines = await File.ReadAllLinesAsync(filePath);
        foreach (var line in lines.Skip(1)) {
            var values = line.Split(',');

            StudentsSubjectsStates stateEnum = values[4].ToLower() switch {
                "oncourse" => StudentsSubjectsStates.ONCOURSE,
                "final" => StudentsSubjectsStates.FINAL,
                "passed" => StudentsSubjectsStates.PASSED,
                _ => throw new Exception($"Valor inválido para plan: {values[4]}")
            };

            var division = new StudentSubject(
                studentId: int.Parse(values[0]),
                subjectId: int.Parse(values[1]),
                classId: int.Parse(values[2]),
                grade: string.IsNullOrWhiteSpace(values[3]) || values[3].ToUpper() == "NULL"
                ? null
                : int.Parse(values[3]),
                state: stateEnum
            );


            _context.StudentSubjects.Add(division);
        }

        await _context.SaveChangesAsync();
    }
}
