using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thor.Provider.SqlServer.Logger
{
    /// <inheritdoc />
    public partial class AddOrganizationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Loggers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_OrganizationId",
                table: "Loggers",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loggers_OrganizationId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Loggers");
        }
    }
}
