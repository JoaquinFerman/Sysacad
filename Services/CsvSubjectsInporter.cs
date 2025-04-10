using Sysachad.Models;

public class CsvSubjectsImporter
{
    private readonly UniversidadContext _context;

    public CsvSubjectsImporter(UniversidadContext context)
    {
        _context = context;
    }

    public async Task ImportFromCsv(string filePath)
    {
        if (_context.Subjects.Any()) {
            Console.WriteLine("La tabla Subjects ya tiene datos. No se importará nada.");
            return;
        }
        
        var lines = await File.ReadAllLinesAsync(filePath);
        foreach (var line in lines.Skip(1))
        {
            var values = line.Split(',');

            SubjectsPlan planEnum = values[5] switch {
                "P2024" => SubjectsPlan.P2024,
                "P2003" => SubjectsPlan.P2003,
                _ => throw new Exception($"Valor inválido para plan: {values[5]}")
            };

            var subject = new Subject(
                sId: int.Parse(values[0]),
                name: values[1],
                duration: values[2],
                attendance: values[3],
                year: int.Parse(values[4]),
                plan: planEnum
            );


            if (!_context.Subjects.Any(s => s.SId == subject.SId)) {
                _context.Subjects.Add(subject);
            }
        }

        await _context.SaveChangesAsync();
    }
}
