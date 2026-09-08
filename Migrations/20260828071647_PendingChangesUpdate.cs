using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class PendingChangesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ContactInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ContactInfos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
