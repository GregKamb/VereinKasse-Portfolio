using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Απαραίτητο για το SelectList
using Microsoft.EntityFrameworkCore;
using VereinKasseScaffolding.Models;
using VereinKasseScaffolding.NewFolder;
using VereinKasseScaffolding.Services; // Ή NewFolder

namespace VereinKasseScaffolding.Controllers
{
    public class ZahlungController : Controller
    {
        private readonly ZahlungService _zahlungService;
        private readonly MitgliedService _mitgliedService;

        public ZahlungController(ZahlungService zahlungService, MitgliedService mitgliedService)
        {
            _zahlungService = zahlungService;
            _mitgliedService = mitgliedService;
        }

        // GET: Zahlung (Index με Αναζήτηση)
        public async Task<IActionResult> Index(string search, int typeFilter, int mitgliedId,DateTime? datumVon, DateTime? datumBis, int numEinträge)
        {
            var vm = new ZahlungIndexVm();
            vm.Suchbegriff = search;
            vm.TypeFilter = typeFilter;
            vm.MitgliedId = mitgliedId;

            // Αν δεν έχει οριστεί, θέτουμε το "DatumVon" στην αρχή του τρέχοντος έτους και το "DatumBis" στο τέλος του τρέχοντος έτους
            // Αυτό εξασφαλίζει ότι όταν ο χρήστης φορτώνει τη σελίδα για πρώτη φορά, θα βλέπει τις πληρωμές του τρέχοντος έτους
            vm.DatumVon = datumVon;
            vm.DatumBis = datumBis;

            vm.Zahlungen = await _zahlungService.GetFilteredZahlungenAsync(search, typeFilter, mitgliedId, datumVon, datumBis, numEinträge);
            vm.Mitglieder = await _mitgliedService.GetAlleMitgliederAsync();

            return View(vm);
        }

        // GET: Zahlung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var zahlung = await _zahlungService.GetZahlungByIdAsync(id.Value);
            if (zahlung == null) return NotFound();

            return View(zahlung);
        }

        // GET: Zahlung/Create
        public async Task<IActionResult> Create()
        {
            // Φέρνουμε τα μέλη για να γεμίσουμε το Dropdown
            var mitglieder = await _mitgliedService.GetAlleMitgliederAsync();
            ViewData["MitgliedId"] = new SelectList(mitglieder, "Id", "Nachname");
            return View();
        }

        // POST: Zahlung/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Betrag,Datum,Beschreibung,IstEinzahlung,MitgliedId")] Zahlung zahlung)
        {
            // Προσθέσαμε έναν επιπλέον έλεγχο για να βεβαιωθούμε ότι η ημερομηνία δεν είναι στο μέλλον
            if(zahlung.Datum > DateTime.Now)
            {
                ModelState.AddModelError("Datum", "Das Datum darf nicht in der Zukunft liegen.");
            }


            if ( ModelState.IsValid)
            {
                await _zahlungService.AddZahlungAsync(zahlung);
                return RedirectToAction(nameof(Index));
            }

            // Αν κάτι πήγε στραβά, ξαναφορτώνουμε τη λίστα με τα μέλη
            var mitglieder = await _mitgliedService.GetAlleMitgliederAsync();
            ViewData["MitgliedId"] = new SelectList(mitglieder, "Id", "Nachname", zahlung.MitgliedId);
            return View(zahlung);
        }

        // GET: Zahlung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var zahlung = await _zahlungService.GetZahlungByIdAsync(id.Value);
            if (zahlung == null) return NotFound();

            // Φόρτωσε τα μέλη για το Dropdown
            var mitglieder = await _mitgliedService.GetAlleMitgliederAsync();
            ViewData["MitgliedId"] = new SelectList(mitglieder, "Id", "Nachname", zahlung.MitgliedId);
            return View(zahlung);
        }

        // POST: Zahlung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Betrag,Datum,Beschreibung,IstEinzahlung,MitgliedId")] Zahlung zahlung)
        {
            if (id != zahlung.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _zahlungService.UpdateZahlungAsync(zahlung);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _zahlungService.ZahlungExistsAsync(zahlung.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var mitglieder = await _mitgliedService.GetAlleMitgliederAsync();
            ViewData["MitgliedId"] = new SelectList(mitglieder, "Id", "Nachname", zahlung.MitgliedId);
            return View(zahlung);
        }

        // GET: Zahlung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var zahlung = await _zahlungService.GetZahlungByIdAsync(id.Value);
            if (zahlung == null) return NotFound();

            return View(zahlung);
        }

        // POST: Zahlung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _zahlungService.DeleteZahlungAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Zahlung/Statistic
        public async Task<IActionResult> Statistic(DateTime? datumVon, DateTime? datumBis, int mitgliedId)
        {
            // 1. Ζητάμε από το Service να μας φέρει τα έτοιμα στατιστικά
            // (Το Service θα κάνει τους υπολογισμούς και θα γεμίσει τα Arrays για το γράφημα)
            var vm = await _zahlungService.GetKassaStatisticsAsync(datumVon, datumBis, mitgliedId);

            // 2. "Καρφώνουμε" ξανά τις τιμές των φίλτρων στο ViewModel
            // Αυτό το κάνουμε για να μην χαθούν οι επιλογές του χρήστη μόλις πατήσει "Search"
            vm.DatumVon = datumVon;
            vm.DatumBis = datumBis;
            vm.MitgliedId = mitgliedId;

            // 3. Φέρνουμε τη λίστα με ΟΛΑ τα μέλη για να γεμίσουμε το Dropdown στη φόρμα
            // Αν ξεχάσουμε αυτή τη γραμμή, το Dropdown θα είναι άδειο!
            vm.AlleMitglieder = await _mitgliedService.GetAlleMitgliederAsync();

            // 4. Στέλνουμε το πακέτο στο View
            return View(vm);
        }

    }
}