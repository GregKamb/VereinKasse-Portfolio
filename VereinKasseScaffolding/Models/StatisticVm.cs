namespace VereinKasseScaffolding.Models
{
    public class StatisticVm
    {
        // Wir verwenden "string" für die Datenbeschriftungen, da sie in der Regel Textinformationen enthalten (z.B. Kategorienamen, Zeiträume, etc.).
        // Wir initialisieren die Arrays mit "Array.Empty<>()", um sicherzustellen,
        // dass sie nicht null sind, auch wenn keine Daten vorhanden sind.
        public string[] DataLabels { get; set; } = Array.Empty<string>();
        //Wir verwenden "int" für die Datenwerte, da sie in der Regel numerische
        //Informationen enthalten (z.B. Anzahl der Mitglieder, Summe der Zahlungen, etc.).
        public int[] DataValues { get; set; } = Array.Empty<int>();
    }
}

