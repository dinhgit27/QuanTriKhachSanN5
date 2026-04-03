using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanTriKhachSanN5.Migrations
{
    /// <inheritdoc />
    public partial class AddAttractionsEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GooglePlaceId",
                table: "Attractions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attractions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Attractions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Attractions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "Attractions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttractionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CloudinaryPublicId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttractionImages_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionImages_AttractionId",
                table: "AttractionImages",
                column: "AttractionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionImages");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "GooglePlaceId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attractions");
        }
    }
}
