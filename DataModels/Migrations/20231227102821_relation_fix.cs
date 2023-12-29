using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataModels.Migrations
{
    /// <inheritdoc />
    public partial class relation_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Constructions_Projects_ProjectID",
                table: "Constructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Projects_ProjectID",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ProjectID",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Constructions_ProjectID",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Constructions");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionID",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ConstructionID",
                table: "Projects",
                column: "ConstructionID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationID",
                table: "Projects",
                column: "OrganizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Constructions_ConstructionID",
                table: "Projects",
                column: "ConstructionID",
                principalTable: "Constructions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_OrganizationID",
                table: "Projects",
                column: "OrganizationID",
                principalTable: "Organizations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Constructions_ConstructionID",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_OrganizationID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ConstructionID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConstructionID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Organizations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Constructions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ProjectID",
                table: "Organizations",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_ProjectID",
                table: "Constructions",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Constructions_Projects_ProjectID",
                table: "Constructions",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Projects_ProjectID",
                table: "Organizations",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID");
        }
    }
}
