using Microsoft.EntityFrameworkCore;
using VereinKasseScaffolding.Data;
using VereinKasseScaffolding.Models;
namespace VereinKasseScaffolding.Services
{
    public class ZahlungService
    {
        // DbContext-Instanz für den Datenbankzugriff
        private readonly VereinContext _context;

        // Konstruktor : Zuweisung des DbContext
        public ZahlungService(VereinContext context)
        {
            _context = context;
        }

        // Methode zum Abrufen aller Zahlungen
        public async Task<List<Models.Zahlung>> GetAlleZahlungenAsync()
        {
            return await _context.Zahlungen
                .Include(z => z.Mitglied) // Optional: Laden der zugehörigen Mitglied-Daten
                .ToListAsync();
        }

        public async Task<List<Zahlung>> GetFilteredZahlungenAsync(
            string search, 
            int typeFilter, 
            int mitgliedId, 
            DateTime? datumVon, 
            DateTime? datumBis,
            int numEinträge)
        {
            // Ξεκινάμε με όλες τις πληρωμές ΚΑΙ τα μέλη τους
            // (Include για να έχουμε πρόσβαση στα δεδομένα του μέλους, π.χ. για το φίλτρο μέλους)
            var query = _context.Zahlungen.Include(z => z.Mitglied).AsQueryable();

            // 1. Αναζήτηση στην Περιγραφή (Beschreibung)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(z => z.Beschreibung.Contains(search));
            }

            // 2. Φίλτρο Τύπου (1 = Εισπράξεις, 2 = Πληρωμές)
            if (typeFilter == 1) // Μόνο Εισπράξεις (IstEinzahlung == true)
            {
                query = query.Where(z => z.IstEinzahung == true);
            }
            else if (typeFilter == 2) // Μόνο Πληρωμές (IstEinzahlung == false)
            {
                query = query.Where(z => z.IstEinzahung == false);
            }

            // 3. Φίλτρο Μέλους (Αν επιλέξαμε συγκεκριμένο άτομο)
            if (mitgliedId != 0)
            {
                query = query.Where(z => z.MitgliedId == mitgliedId);
            }

            // 4. Φίλτρο Ημερομηνίας
            if (datumVon.HasValue) //!=null
            {
                query = query.Where(z => z.Datum >= datumVon.Value);
            }
            if (datumBis.HasValue)
            {
                var nextDay = datumBis.Value.AddDays(1); // Για να συμπεριλάβουμε και την ημερομηνία "bis"
                query = query.Where(z => z.Datum < nextDay);
            }



            // Ταξινόμηση: Τα πιο πρόσφατα πρώτα (όπως ζητάει η άσκηση)
            query = query.OrderByDescending(z => z.Datum);

            // 5. Περιορισμός Αριθμού Εγγραφών (αν έχει οριστεί)
            if (numEinträge > 0)
            {
                query = query.Take(numEinträge);
            }

            return await query.ToListAsync();
        }

        // Χρειαζόμαστε αυτή τη μέθοδο για το Edit και το Delete
        public async Task<Zahlung?> GetZahlungByIdAsync(int id)
        {
            return await _context.Zahlungen
                                 .Include(z => z.Mitglied)
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        // --- 2. ΕΓΓΡΑΦΗ (CREATE) ---
        public async Task AddZahlungAsync(Zahlung zahlung)
        {
            _context.Zahlungen.Add(zahlung);
            await _context.SaveChangesAsync();
        }

        // --- 3. ΕΝΗΜΕΡΩΣΗ (UPDATE) ---
        public async Task UpdateZahlungAsync(Zahlung zahlung)
        {
            _context.Zahlungen.Update(zahlung);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ZahlungExistsAsync(int id)
        {
            return await _context.Zahlungen.AnyAsync(e => e.Id == id);
        }

        // --- 4. ΔΙΑΓΡΑΦΗ (DELETE) ---
        public async Task DeleteZahlungAsync(int id)
        {
            var zahlung = await GetZahlungByIdAsync(id);
            if (zahlung != null)
            {
                _context.Zahlungen.Remove(zahlung);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<KassaStatisticVm> GetKassaStatisticsAsync(DateTime? datumVon, DateTime? datumBis, int mitgliedId)
        {
            var vm = new KassaStatisticVm();

            // Ξεκινάμε με όλες τις πληρωμές ΚΑΙ τα μέλη τους
            var query = _context
                .Zahlungen
                .Include(z=> z.Mitglied)
                .AsQueryable();
            
            if(mitgliedId != 0)
            {
                query = query.Where(z => z.MitgliedId == mitgliedId);
            }
            if (datumVon.HasValue)
            {
                query = query.Where(z => z.Datum >= datumVon.Value);
            }
            if (datumBis.HasValue)
            {
                // Για να συμπεριλάβουμε και την ημερομηνία "bis", προσθέτουμε 1 μέρα και κάνουμε < αντί για <=
                var nextDay = datumBis.Value.AddDays(1);
                query = query.Where(z => z.Datum < nextDay);
            }

            var getfilterteListe = await query.ToListAsync();
            // Για τις εισπράξεις (Einnahmen) κάνουμε το φίλτρο για IstEinzahlung == true
            var einzahlungen = getfilterteListe.Where(z => z.IstEinzahung == true).ToList();
            vm.EinnahmenSumme = einzahlungen.Sum(z => z.Betrag);
            vm.EinnahmenAnzahl = einzahlungen.Count();

            // Για τις πληρωμές (Auszahlungen) κάνουμε το αντίθετο φίλτρο         
            var auszahlungen = getfilterteListe.Where(z => z.IstEinzahung == false).ToList();
            vm.AusgabenSumme = auszahlungen.Sum(z => z.Betrag);
            vm.AusgabenAnzahl = auszahlungen.Count();

            // Η διαφορά (Bilanz) είναι απλά η διαφορά μεταξύ των δύο προηγούμενων
            vm.BilanzSumme = vm.EinnahmenSumme - vm.AusgabenSumme;
            // Ο αριθμός των εγγραφών για τη Bilanz είναι το άθροισμα των δύο προηγούμενων
            vm.BilanzAnzahl = getfilterteListe.Count(); // Εναλλακτικά: vm.EinnahmenAnzahl + vm.AusgabenAnzahl

            // Για τα Top 5 Einzahlungen (με βάση το ποσό, φιλτράρουμε μόνο τις εισπράξεις και μετά παίρνουμε τις 5 μεγαλύτερες)
            var top5Ein = einzahlungen // εχει ολα τα δεδομενα με τα φιλτρα που εχουμε βαλει
                .Where(z => z.IstEinzahung == true)
                .OrderByDescending(z => z.Betrag)
                .Take(5)
                .ToList();

            vm.Top5EinLabels = top5Ein
                .Select(z=> (z.Mitglied != null ? z.Mitglied!.Nachname : "") + " - " + z.Beschreibung)
                .ToArray(); // Ετικέτες: "Επώνυμο Μέλους - Περιγραφή"

            vm.Top5EinValues = top5Ein
                .Select(z => z.Betrag)
                .ToArray(); // Τιμές: Τα ποσά των 5 μεγαλύτερων εισπράξεων

            var top5Aus = auszahlungen
                .Where(z => z.IstEinzahung == false)
                .OrderByDescending(z => z.Betrag)
                .Take(5)
                .ToList();

            vm.Top5AusLabels = top5Aus
                .Select(z => (z.Mitglied != null ? z.Mitglied!.Nachname : "") + " - " + z.Beschreibung)
                .ToArray(); // Ετικέτες: "Επώνυμο Μέλους - Περιγραφή"

            vm.Top5AusValues = top5Aus
                .Select(z => z.Betrag)
                .ToArray(); // Τιμές: Τα ποσά των 5 μεγαλύτερων πληρωμών

            return vm;


        }
    }
}
