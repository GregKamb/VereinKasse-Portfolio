using VereinKasseScaffolding.Models;
 using Microsoft.EntityFrameworkCore;   
namespace VereinKasseScaffolding.Data


{
    public class VereinContext : DbContext
    {

        // Konstruktor : Übergabe der Optionen an die Basisklasse
        public VereinContext(DbContextOptions<VereinContext> options) : base(options)
            { 
            }
        // DbSets für die Entitäten
        // DbSets για τις οντότητες
        public DbSet<Mitglied> Mitglieder { get; set; } = null!;
        public DbSet<Zahlung> Zahlungen { get; set; } = null!;
    }
}
