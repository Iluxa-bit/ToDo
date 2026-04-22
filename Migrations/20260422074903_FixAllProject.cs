using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_1.Migrations
{
    /// <inheritdoc />
    public partial class FixAllProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "Logs",
                newName: "DateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Logs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Logs",
                newName: "dateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "Logs",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
