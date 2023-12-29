using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataModels.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Projects",
                type: "TEXT",
                precision: 19,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedPrice",
                table: "Constructions",
                type: "TEXT",
                precision: 19,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Projects",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 19,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedPrice",
                table: "Constructions",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 19,
                oldScale: 8);
        }
    }
}
