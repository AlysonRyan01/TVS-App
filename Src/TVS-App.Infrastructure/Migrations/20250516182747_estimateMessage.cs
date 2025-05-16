using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVS_App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class estimateMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstimateMessage",
                table: "ServiceOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimateMessage",
                table: "ServiceOrders");
        }
    }
}
