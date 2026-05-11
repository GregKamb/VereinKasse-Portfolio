using Microsoft.EntityFrameworkCore;
using VereinKasseScaffolding.Data;
using VereinKasseScaffolding.Models;
using VereinKasseScaffolding.NewFolder;
using VereinKasseScaffolding.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Konfiguration des DbContext mit SQL Server
// Ρύθμιση του DbContext με SQL Server
builder.Services.AddDbContext<VereinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Registrierung des MitgliedService für die Abhängigkeitsinjektion
// Εγγραφή της υπηρεσίας MitgliedService για ένεση εξαρτήσεων
builder.Services.AddScoped<MitgliedService>();
builder.Services.AddScoped<ZahlungService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// DATA SEEDING 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VereinContext>();

    // Ελέγχει αν υπάρχει η βάση. Αν δεν υπάρχει, τη φτιάχνει αυτόματα!
    context.Database.EnsureCreated();

    // Αν δεν υπάρχουν καθόλου μέλη, βάλε τα παρακάτω:
    if (!context.Mitglieder.Any())
    {
        // 1. Δημιουργία Μελών (Γερμανικά Ονόματα)
        var m1 = new Mitglied { Vorname = "Max", Nachname = "Mustermann", IstAktiv = true, Eintrittsdatum = DateTime.Now.AddMonths(-10) };
        var m2 = new Mitglied { Vorname = "Anna", Nachname = "Müller", IstAktiv = true, Eintrittsdatum = DateTime.Now.AddMonths(-5) };
        var m3 = new Mitglied { Vorname = "Klaus", Nachname = "Schmidt", IstAktiv = false, Eintrittsdatum = DateTime.Now.AddYears(-2) };
        var m4 = new Mitglied { Vorname = "Julia", Nachname = "Wagner", IstAktiv = true, Eintrittsdatum = DateTime.Now.AddDays(-20) };

        context.Mitglieder.AddRange(m1, m2, m3, m4);
        context.SaveChanges();

        // 2. Δημιουργία Πληρωμών / Ταμείου (Γερμανικές περιγραφές)
        var z1 = new Zahlung { MitgliedId = m1.Id, Betrag = 50.00m, Datum = DateTime.Now.AddDays(-15), Beschreibung = "Jahresbeitrag 2024", IstEinzahung = true };
        var z2 = new Zahlung { MitgliedId = m2.Id, Betrag = 25.00m, Datum = DateTime.Now.AddDays(-5), Beschreibung = "Spende", IstEinzahung = true };
        var z3 = new Zahlung { MitgliedId = m1.Id, Betrag = 15.00m, Datum = DateTime.Now.AddDays(-2), Beschreibung = "T-Shirt Kauf", IstEinzahung = true };
        var z4 = new Zahlung { MitgliedId = m4.Id, Betrag = 50.00m, Datum = DateTime.Now.AddDays(-1), Beschreibung = "Jahresbeitrag 2024", IstEinzahung = true };
        var z5 = new Zahlung { MitgliedId = m3.Id, Betrag = 120.00m, Datum = DateTime.Now.AddDays(-10), Beschreibung = "Saalmiete (Ausgabe)", IstEinzahung = false };

        context.Zahlungen.AddRange(z1, z2, z3, z4, z5);
        context.SaveChanges();
    }
}
// --- END DATA SEEDING ---

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mitglied}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
