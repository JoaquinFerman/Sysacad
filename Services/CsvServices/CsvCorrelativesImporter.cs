using Sysachad.Models;

public class CsvCorrelativesImporter {
    private readonly UniversidadContext _context;

    public CsvCorrelativesImporter(UniversidadContext context) {
        _context = context;
    }

    public async Task ImportFromCsv(string filePath) {
        if (_context.Correlatives.Any()) {
            Console.WriteLine("La tabla Correlatives ya tiene datos. No se importará nada.");
            return;
        }
        
        var lines = await File.ReadAllLinesAsync(filePath);
        foreach (var line in lines.Skip(1)) {
            var values = line.Split(',');

            var correlative = new Correlative(
                sId: int.Parse(values[0]),
                cId: int.Parse(values[1]),
                type: values[2],
                typeFor: values[3]
            );


            _context.Correlatives.Add(correlative);
        }

        await _context.SaveChangesAsync();
    }
}
