using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLimitePrestamosToRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LimitePrestamos",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificadoEn",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "CreadoPor", "LimitePrestamos", "ModificadoEn", "ModificadoPor" },
                values: new object[] { new DateTime(2026, 8, 7, 16, 11, 2, 695, DateTimeKind.Local).AddTicks(4783), "", 10, null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreadoEn", "CreadoPor", "LimitePrestamos", "ModificadoEn", "ModificadoPor" },
                values: new object[] { new DateTime(2026, 8, 7, 16, 11, 2, 695, DateTimeKind.Local).AddTicks(4800), "", 3, null, null });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 7, 16, 11, 2, 951, DateTimeKind.Local).AddTicks(6084), "$2a$11$UiQKW8mTo7QTBTtRl7ZcAOr3NbrTr7UJifsjj8DgMAxn8.6ty0ts6" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LimitePrestamos",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Roles");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreadoEn", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 7, 10, 52, 57, 357, DateTimeKind.Local).AddTicks(7314), "$2a$11$f9sUxbMPDsLxGfKJIi1uTOlp1abBPk26Z0A78LemnkmKE8UomcMni" });
        }
    }
}
