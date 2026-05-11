namespace VereinKasseScaffolding.Models
{
    public class KassaStatisticVm
    {
        //Φιλτρ για την αναζήτηση των κινήσεων της ταμειακής μηχανής
        public DateTime? DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }
        public int MitgliedId { get; set; }
        public List<Mitglied> AlleMitglieder { get; set; } = new List<Mitglied>();

        //Στατιστικά στοιχεία για τις κινήσεις της ταμειακής μηχανής
        public decimal EinnahmenSumme { get; set; }
        public int EinnahmenAnzahl { get; set; } //Σύνολο των εσόδων και αριθμός των κινήσεων εσόδων
        public decimal AusgabenSumme { get; set; }
        public int AusgabenAnzahl { get; set; } //Σύνολο των εξόδων και αριθμός των κινήσεων εξόδων
        public decimal BilanzSumme { get; set; } //Σύνολο των εσόδων μείον τα έξοδα
        public int BilanzAnzahl { get; set; } //Σύνολο των κινήσεων (έσοδα + έξοδα)

        //Στατιστικά στοιχεία για τις 5 κορυφαίες κατηγορίες κινήσεων της ταμειακής μηχανής
        public string[] Top5EinLabels { get; set; } = Array.Empty<string>(); //Ετικέτες για τις 5 κορυφαίες κατηγορίες κινήσεων
        public decimal[] Top5EinValues { get; set; } = Array.Empty<decimal>(); //Τιμές για τις 5 κορυφαίες κατηγορίες κινήσεων

        //Στατιστικά στοιχεία για τις 5 κορυφαίες κατηγορίες κινήσεων της ταμειακής μηχανή
        public string[] Top5AusLabels { get; set; } = Array.Empty<string>(); //Ετικέτες για τις 5 κορυφαίες κατηγορίες κινήσεων
        public decimal[] Top5AusValues { get; set; } = Array.Empty<decimal>(); //Τιμές για τις 5 κορυφαίες κατηγορίες κινήσεων
    }
}

