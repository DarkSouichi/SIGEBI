using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLeidaToNotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leida",
                table: "Notificaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 7, 10, 52, 57, 357, DateTimeKind.Local).AddTicks(7314), "$2a$11$f9sUxbMPDsLxGfKJIi1uTOlp1abBPk26Z0A78LemnkmKE8UomcMni" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leida",
                table: "Notificaciones");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 23, 32, 44, 693, DateTimeKind.Local).AddTicks(3501), "$2a$11$rnthPNnNUJRM1iJRhGTLheLOxTboqx53vJddnyXdyahnEkABgaPSK" });
        }
    }
}
