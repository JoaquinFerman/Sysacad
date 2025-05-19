namespace Sysachad.Models {
    /// <summary>
    /// Represents the model of the return in the case of a correlative from the controllers
    /// </summary>
    public class CorrelativeReturn {
        public int Year { get; set; }
        public string Name { get; set; }
        public List<string> Condition { get; set; }
        public string Plan { get; set; }
    }
}