using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxy.Security.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class codigounico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reclamos_Codigo",
                table: "Reclamos",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reclamos_Codigo",
                table: "Reclamos");
        }
    }
}
