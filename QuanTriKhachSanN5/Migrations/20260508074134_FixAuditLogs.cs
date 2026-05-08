using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanTriKhachSanN5.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "deposit_amount",
                table: "Bookings",
                newName: "DepositAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepositAmount",
                table: "Bookings",
                newName: "deposit_amount");
        }
    }
}
