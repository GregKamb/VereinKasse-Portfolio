namespace VereinKasseScaffolding.Models
{
    public class ZahlungIndexVm
    {
        public List<Zahlung> Zahlungen { get; set; } = new List<Zahlung>();
        public List<Mitglied> Mitglieder { get; set; } = new List<Mitglied>();
        public string Suchbegriff { get; set; } = string.Empty;
        public int TypeFilter { get; set; } = 0; // 0 = Alle, 1 = Einzahlungen, 2 = Auszahlungen
        public int MitgliedId { get; set; } = 0; // 0 = Alle Mitglieder
        public DateTime? DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }
        public int NumEinträge { get; set; }
    }
}
