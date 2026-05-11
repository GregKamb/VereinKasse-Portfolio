namespace VereinKasseScaffolding.Models
{
    public class MitgliedIndexVm
    {
        public List<Mitglied> Mitglieder { get; set; } = new List<Mitglied>();
        public string Suchbegriff { get; set; } = string.Empty;
        public int FilterStatus { get; set; } = 0; // 0 = Alle, 1 = Aktiv, 2 = Inaktiv

        public DateTime? DatumVon { get; set; }
        //Αν θέλεις να έχεις και ένα πεδίο για το τέλος της ημερομηνίας, μπορείς να προσθέσεις και αυτό
        //Γιατί: Το View (η σελίδα HTML) πρέπει κάπως να στείλει στον Controller τις ημερομηνίες που επέλεξε ο χρήστης.
        //Το ViewModel είναι το "πακέτο" που μεταφέρει αυτές τις επιλογές.
        public DateTime? DatumBis { get; set; }

    }
}

