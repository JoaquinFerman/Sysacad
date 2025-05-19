using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sysachad.Models {
    /// <summary>
    /// Represents a subject required for other one
    /// </summary>
    public class Correlative {
        [Key]
        public int Id { get; set; } = 0;
        [Required]
        public int SId { get; set; }
        public int CId { get; set; }

        public bool TypeBin { get; set; }
        [NotMapped]
        public string Type => TypeBin ? "Rendir" : "Cursar"; // que tiene que haber sido la materia
        public bool TypeBinFor { get; set; } // para hacer que
        [NotMapped]
        public string TypeFor => TypeBinFor ? "Rendir" : "Cursar";

        public Correlative(int sId, int cId, string type, string typeFor) : this() {
            SId = sId;
            CId = cId;
            TypeBin = type == "Rendir" ? true : false;
            TypeBinFor = typeFor == "Rendir" ? true : false;
        }

        public Correlative() {}
    }
}
