namespace VereinKasseScaffolding.Models
{
    public class Mitglied
    {
        public int Id { get; set; }
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public bool IstAktiv { get; set; }
        public string? Bild { get; set; } = "";
        //Wir setzen "= DateTime.Now",
        //damit das Programm automatisch das aktuelle Datum abruft, falls wir es vergessen.
        public DateTime Eintrittsdatum { get; set; } = DateTime.Now; 
    }
}

