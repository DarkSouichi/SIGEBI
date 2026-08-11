using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDescripcionFechaLanzamientoToRecurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Recursos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLanzamiento",
                table: "Recursos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 4, 19, 50, 54, 366, DateTimeKind.Local).AddTicks(5499), "$2a$11$lUlbjS0FAkX.yK9lvO5uiumZ6lu62Q8tEq2KJLr08jJuC9kBUWmKK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "FechaLanzamiento",
                table: "Recursos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 2, 16, 1, 4, 618, DateTimeKind.Local).AddTicks(7937), "$2a$11$0lfkdEbYY6U0CTbs84nuFOalyk3Eoqsi7xQNVul6FLTTdP8StK8P." });
        }
    }
}
