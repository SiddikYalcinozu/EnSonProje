using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnSonProje.Migrations
{
    /// <inheritdoc />
    public partial class EmanetTablosuEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emanetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OgrenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    KitapId = table.Column<int>(type: "INTEGER", nullable: false),
                    VerilisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TeslimTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emanetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emanetler_Kitaplar_KitapId",
                        column: x => x.KitapId,
                        principalTable: "Kitaplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emanetler_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emanetler_KitapId",
                table: "Emanetler",
                column: "KitapId");

            migrationBuilder.CreateIndex(
                name: "IX_Emanetler_OgrenciId",
                table: "Emanetler",
                column: "OgrenciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emanetler");
        }
    }
}
