using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanTriKhachSanN5.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs");

            migrationBuilder.AddColumn<int>(
                name: "membership_id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "points",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Audit_Logs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "log_data",
                table: "Audit_Logs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_membership_id",
                table: "Users",
                column: "membership_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Memberships_membership_id",
                table: "Users",
                column: "membership_id",
                principalTable: "Memberships",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Memberships_membership_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_membership_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "membership_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "points",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Audit_Logs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "log_data",
                table: "Audit_Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
