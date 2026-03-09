using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnSonProje.Migrations
{
    /// <inheritdoc />
    public partial class KitapModeliGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YayınEvi",
                table: "Kitaplar",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YayınEvi",
                table: "Kitaplar");
        }
    }
}
