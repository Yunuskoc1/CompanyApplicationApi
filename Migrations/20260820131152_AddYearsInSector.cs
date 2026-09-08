using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddYearsInSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearslnSector",
                table: "CompanyInfos",
                newName: "YearsInSector");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearsInSector",
                table: "CompanyInfos",
                newName: "YearslnSector");
        }
    }
}
