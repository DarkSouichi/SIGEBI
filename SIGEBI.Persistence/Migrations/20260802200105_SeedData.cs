using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nombre", "Permisos" },
                values: new object[,]
                {
                    { 1, "Admin", "Todos" },
                    { 2, "Usuario", "Lectura" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "CreadoEn", "CreadoPor", "Email", "EstaActivo", "IntentosFallidos", "ModificadoEn", "ModificadoPor", "NombreCompleto", "PasswordHash", "RolId" },
                values: new object[] { 1, new DateTime(2026, 8, 2, 16, 1, 4, 618, DateTimeKind.Local).AddTicks(7937), "Sistema", "admin@test.com", true, 0, null, null, "Administrador", "$2a$11$0lfkdEbYY6U0CTbs84nuFOalyk3Eoqsi7xQNVul6FLTTdP8StK8P.", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
