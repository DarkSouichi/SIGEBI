using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRechazadoEstadoPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreadoEn",
                value: new DateTime(2026, 8, 7, 18, 32, 12, 817, DateTimeKind.Local).AddTicks(1512));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreadoEn",
                value: new DateTime(2026, 8, 7, 18, 32, 12, 817, DateTimeKind.Local).AddTicks(1532));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 7, 18, 32, 13, 73, DateTimeKind.Local).AddTicks(3144), "$2a$11$Hs3o1hL6xAGlYybnMyQ6OeNLAxienJPcoBxtCaAHUIllFryPBE0q." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreadoEn",
                value: new DateTime(2026, 8, 7, 16, 11, 2, 695, DateTimeKind.Local).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreadoEn",
                value: new DateTime(2026, 8, 7, 16, 11, 2, 695, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 7, 16, 11, 2, 951, DateTimeKind.Local).AddTicks(6084), "$2a$11$UiQKW8mTo7QTBTtRl7ZcAOr3NbrTr7UJifsjj8DgMAxn8.6ty0ts6" });
        }
    }
}
