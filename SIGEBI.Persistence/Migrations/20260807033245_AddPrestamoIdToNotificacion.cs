using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPrestamoIdToNotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrestamoId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecursoId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 23, 32, 44, 693, DateTimeKind.Local).AddTicks(3501), "$2a$11$rnthPNnNUJRM1iJRhGTLheLOxTboqx53vJddnyXdyahnEkABgaPSK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrestamoId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "RecursoId",
                table: "Notificaciones");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 4, 19, 50, 54, 366, DateTimeKind.Local).AddTicks(5499), "$2a$11$lUlbjS0FAkX.yK9lvO5uiumZ6lu62Q8tEq2KJLr08jJuC9kBUWmKK" });
        }
    }
}
