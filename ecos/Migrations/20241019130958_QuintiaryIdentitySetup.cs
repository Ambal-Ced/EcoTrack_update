using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecos.Migrations
{
    /// <inheritdoc />
    public partial class QuintiaryIdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ElectricityRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ElectricityRecords");
        }
    }
}
