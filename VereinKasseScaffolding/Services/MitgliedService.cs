using Microsoft.EntityFrameworkCore;
using VereinKasseScaffolding.Data;
using VereinKasseScaffolding.Models;
namespace VereinKasseScaffolding.NewFolder
{
    public class MitgliedService
    {
        // DbContext-Instanz für den Datenbankzugriff
        // Παράδειγμα DbContext για πρόσβαση στη βάση δεδομένων
        private readonly VereinContext _context;

        // Konstruktor : Zuweisung des DbContext
        // Κατασκευαστής : Ανάθεση του DbContext
        public MitgliedService(VereinContext context)
        {
            _context = context;
        }

        // Methode zum Abrufen aller Mitglieder
        // Μέθοδος 1: Επιστρέφει ΟΛΑ τα μέλη (για τη λίστα)
        public async Task<List<Models.Mitglied>> GetAlleMitgliederAsync()
        {
            return await _context.Mitglieder.ToListAsync();
        }
        public async Task<List<Mitglied>> GetFilteredMitgliederAsync(string search, int filterStatus, DateTime? datumVon, DateTime? datumBis)
        {
            //1. Δημιουργία της βασικής ερώτησης
            // Ακομα δεν μιλαμε με τη βαση δεδομενων, απλα λεμε θα ψαξω στα μελη.
            IQueryable<Mitglied> query = _context.Mitglieder;

            //2. Εφαρμογή φίλτρου αναζήτησης (αν υπάρχει)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Vorname.Contains(search) || m.Nachname.Contains(search));
            }

            //3. Εφαρμογή φίλτρου ημερομηνιών (αν υπάρχουν)
            if ( datumVon !=null)
            {
                query = query.Where(m => m.Eintrittsdatum >= datumVon.Value);
            }
            if (datumBis != null)
            {
                var nextDay = datumBis.Value.AddDays(1);// Προσθέτουμε μια μέρα για να συμπεριλάβουμε και την ημερομηνία "bis" στην αναζήτηση
                query = query.Where(m => m.Eintrittsdatum < nextDay);
            }

            //3. Εφαρμογή φίλτρου κατάστασης (αν υπάρχει)
            if (filterStatus == 1) // Μόνο ενεργά μέλη
            {
                query = query.Where(m => m.IstAktiv);
            }
            else if (filterStatus == 2) // Μόνο ανενεργά μέλη
            {
                query = query.Where(m => !m.IstAktiv);
            }

            // query = query.OrderByDescending(d=> d.Eintrittsdatum); // Ταξινόμηση κατά ημερομηνία εισόδου

            //4. Εκτέλεση της ερώτησης και επιστροφή των αποτελεσμάτων ως λίστα
            return await query.ToListAsync();

        }
        // Βοηθητική μέθοδος για να φέρει ένα μέλος (για Edit/Delete)
        public async Task<Mitglied?> GetMitgliedByIdAsync(int id)
        {
            return await _context.Mitglieder.FirstOrDefaultAsync(m => m.Id == id);
        }

        // Μέθοδος για προσθήκη (Create)
        public async Task AddMitgliedAsync(Mitglied mitglied, string imagePath)
        {
            mitglied.Bild = imagePath; // Αποθήκευση της διαδρομής της εικόνας στο μέλος
            _context.Mitglieder.Add(mitglied);
            await _context.SaveChangesAsync();
        }

        // Μέθοδος για ενημέρωση (Edit)
        public async Task UpdateMitgliedAsync(Mitglied mitglied)
        {
            _context.Mitglieder.Update(mitglied);
            await _context.SaveChangesAsync();
        }

        // Μέθοδος για διαγραφή (Delete)
        public async Task DeleteMitgliedAsync(int id)
        {
            var mitglied = await GetMitgliedByIdAsync(id);
            if (mitglied != null)
            {
                _context.Mitglieder.Remove(mitglied);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MitgliedExistsAsync(int id)
        {
            return await _context.Mitglieder.AnyAsync(e => e.Id == id);
        }
    }
}
