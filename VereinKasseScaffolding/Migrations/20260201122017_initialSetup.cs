using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VereinKasseScaffolding.Migrations
{
    /// <inheritdoc />
    public partial class initialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mitglieder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nachname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IstAktiv = table.Column<bool>(type: "bit", nullable: false),
                    Bild = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mitglieder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zahlungen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Betrag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Beschreibung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MitgliedId = table.Column<int>(type: "int", nullable: false),
                    IstEinzahung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zahlungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zahlungen_Mitglieder_MitgliedId",
                        column: x => x.MitgliedId,
                        principalTable: "Mitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zahlungen_MitgliedId",
                table: "Zahlungen",
                column: "MitgliedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zahlungen");

            migrationBuilder.DropTable(
                name: "Mitglieder");
        }
    }
}
