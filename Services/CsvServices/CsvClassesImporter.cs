using Sysachad.Models;

public class CsvClassesImporter {
    private readonly UniversidadContext _context;

    public CsvClassesImporter(UniversidadContext context) {
        _context = context;
    }

    public async Task ImportFromCsv(string filePath) {
        if (_context.Classes.Any()) {
            Console.WriteLine("La tabla Divisions ya tiene datos. No se importará nada.");
            return;
        }
        
        var lines = await File.ReadAllLinesAsync(filePath);
        foreach (var line in lines.Skip(1)) {
            var values = line.Split(',');

            var division = new Class(
                sId: int.Parse(values[0]),
                cId: int.Parse(values[1]),
                room: int.Parse(values[2]),
                professor: values[3],
                days: int.Parse(values[4]),
                hours: values[5] == "true" ? 2 : 1
            );


            _context.Classes.Add(division);
        }

        await _context.SaveChangesAsync();
    }
}
