using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VereinKasseScaffolding.Data;
using VereinKasseScaffolding.Models;
using VereinKasseScaffolding.NewFolder;

namespace VereinKasseScaffolding.Controllers
{
    public class MitgliedController : Controller
    {
        //private readonly VereinContext _context;
        private readonly MitgliedService _mitgliedService;
        private readonly IWebHostEnvironment _env;

        public MitgliedController(MitgliedService mitgliedService, IWebHostEnvironment env)
        {
            //_context = context;
            _mitgliedService = mitgliedService;
            _env = env;
        }

        // GET: Mitglied
        public async Task<IActionResult> Index(string search, int filterStatus,DateTime? datumVon, DateTime? datumBis)
        {
            var vm = new MitgliedIndexVm();
            vm.Suchbegriff = search;
            vm.FilterStatus = filterStatus;
            vm.DatumVon = datumVon;
            vm.DatumBis = datumBis;

            vm.Mitglieder = await _mitgliedService.GetFilteredMitgliederAsync(search, filterStatus, datumVon, datumBis);           
          
            //var mitglieder = await _mitgliedService.GetAlleMitgliederAsync();
            return View(vm);
        }

        public async Task<IActionResult> Statistic()
        {
            var vm = new StatisticVm();
            var alleMitglieder = await _mitgliedService.GetAlleMitgliederAsync();

            // Παράδειγμα: Πόσα μέλη είναι ενεργά και πόσα όχι
            int aktivCount = alleMitglieder.Count(m => m.IstAktiv);
            // Παράδειγμα: Πόσα μέλη είναι ανενεργά
            int inaktivCount = alleMitglieder.Count(m => !m.IstAktiv);

            // Γεμίζουμε το ViewModel με τα δεδομένα για το γράφημα
            vm.DataLabels = new[] { "Aktiv", "Inaktiv" };

            // Γεμίζουμε το ViewModel με τις τιμές για το γράφημα
            // Σιγουρευόμαστε ότι οι τιμές είναι σε σωστή σειρά (πρώτα ενεργά, μετά ανενεργά)
            vm.DataValues = new[] { aktivCount, inaktivCount };

            return View(vm);


        }

        // GET: Mitglied/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mitglied = await _mitgliedService.GetMitgliedByIdAsync(id.Value);

            if (mitglied == null) return NotFound();

            return View(mitglied);
        }

        // GET: Mitglied/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mitglied/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mitglied mitglied, IFormFile bildDatei)
        {
            // Αρχικοποίηση του path (όπως στο παράδειγμα: var relativeImagePath = "";)
            var relativeImagePath = "";

            if (ModelState.IsValid)
            {
                // 1. Λογική Upload (Ακριβώς όπως στο NewItem του WebAppMitCustomDb)
                if (bildDatei != null && bildDatei.Length > 0)
                {
                    // Φτιάχνουμε το σχετικό path για τη βάση: "/images/onoma.jpg"
                    relativeImagePath = $"/images/{bildDatei.FileName}";

                    // Φτιάχνουμε το απόλυτο path για τον δίσκο
                    // Χρησιμοποιούμε το _env.WebRootPath που είναι πιο ασφαλές από το ContentRootPath + "/wwwroot"
                    var fullImagePath = Path.Combine(_env.WebRootPath, "images", bildDatei.FileName);

                    // Αποθήκευση αρχείου (FileStream)
                    using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
                    {
                        await bildDatei.CopyToAsync(fileStream);
                    }
                }

                // 2. Κλήση του Service (Περνάμε το αντικείμενο ΚΑΙ το path ξεχωριστά)
                // Αυτό είναι που ήθελες: να μοιάζει με το _todoService.CreateNewTodoItemAsync(...)
                await _mitgliedService.AddMitgliedAsync(mitglied, relativeImagePath);

                return RedirectToAction(nameof(Index));
            }

            return View(mitglied);
        }

        // GET: Mitglied/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mitglied = await _mitgliedService.GetMitgliedByIdAsync(id.Value);
            if (mitglied == null)
            {
                return NotFound();
            }
            return View(mitglied);
        }

        // POST: Mitglied/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vorname,Nachname,IstAktiv,Bild")] Mitglied mitglied)
        {
            if (id != mitglied.Id) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try
                {
                await _mitgliedService.UpdateMitgliedAsync(mitglied);

                }
                catch (DbUpdateConcurrencyException)
                {
                    // Τώρα χρησιμοποιούμε το await επειδή η μέθοδος είναι async
                    bool exists = await _mitgliedService.MitgliedExistsAsync(mitglied.Id);
                    

                        if (!exists)
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
            return View(mitglied);
        }

        // GET: Mitglied/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mitglied = await _mitgliedService.GetMitgliedByIdAsync(id.Value);

            if (mitglied == null) { return NotFound(); }

            return View(mitglied);
        }

        // POST: Mitglied/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await _mitgliedService.DeleteMitgliedAsync(id);
            return RedirectToAction(nameof(Index));
        }        

    }
}       
