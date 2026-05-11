# VereinKasse - Vereinsverwaltung & Kassenbuch

Ein ASP.NET Core MVC Projekt zur einfachen Verwaltung von Vereinsmitgliedern und der Vereinskasse. Entwickelt als Portfolio-Projekt, um meine Fähigkeiten in C#, ASP.NET Core, Entity Framework Core und Bootstrap zu demonstrieren.

## 🚀 Features

* **Mitgliederverwaltung:** * Hinzufügen, Bearbeiten und Löschen von Vereinsmitgliedern.
    * Suchfunktion nach Vor- oder Nachname.
    * Filterung nach Aktiv/Inaktiv-Status.
* **Kassenbuch (Zahlungen):** * Erfassen von Einzahlungen (z.B. Jahresbeiträge, Spenden) und Auszahlungen (z.B. Mieten, Ausrüstung).
    * Verknüpfung jeder Transaktion mit einem bestimmten Mitglied.
    * Filterung nach Datum, Mitglied und Transaktionstyp.
* **Statistiken & Dashboard:** * Grafische Auswertung der aktiven vs. inaktiven Mitglieder (Chart.js).
    * Übersicht über die Gesamtbilanz, Einnahmen und Ausgaben.

## 🛠️ Verwendete Technologien

* **Backend:** C#, ASP.NET Core MVC (.NET 9/8)
* **Datenbank:** Entity Framework Core, SQL Server (LocalDB/SQLEXPRESS)
* **Frontend:** HTML5, CSS3, Bootstrap 5, Chart.js

## 💡 Für Recruiter / Tester

Das Projekt enthält einen **Data-Seeding-Mechanismus**. 
Wenn Sie die Anwendung zum ersten Mal starten, wird die Datenbank automatisch erstellt und mit **Beispieldaten (Mitglieder & Transaktionen)** gefüllt. Sie können das Programm sofort testen, ohne vorher manuelle Eingaben machen zu müssen!
