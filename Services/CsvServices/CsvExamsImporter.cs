using Sysachad.Models;

public class CsvExamsImporter {
    private readonly UniversidadContext _context;

    public CsvExamsImporter(UniversidadContext context) {
        _context = context;
    }

    public async Task ImportFromCsv(string filePath) {
        if (_context.Exams.Any()) {
            Console.WriteLine("La tabla Exams ya tiene datos. No se importará nada.");
            return;
        }
        
        var lines = await File.ReadAllLinesAsync(filePath);
        foreach (var line in lines.Skip(1)) {
            var values = line.Split(',');

            var exam = new Exam(
                sId: int.Parse(values[0]),
                date: DateTime.Parse(values[1])
            );


            _context.Exams.Add(exam);
        }

        await _context.SaveChangesAsync();
    }
}
