using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VereinKasseScaffolding.Models
{
    public class Zahlung
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Betrag { get; set; }
        public DateTime Datum { get; set; }
        // [MaxLength(50)]
        [StringLength(50)] // ειναι καλυτερο απο το MaxLength για την περιπτωση που θελουμε να εχουμε και validation στο μοντελο
        public string Beschreibung { get; set; } = string.Empty;
        public int MitgliedId { get; set; }
        public Mitglied? Mitglied { get; set; }
        public bool IstEinzahung { get; set; }

    }
}
