using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanTriKhachSanN5.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleDashboardPeriodStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticleCategories_CategoryId",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleCategories",
                table: "ArticleCategories");

            migrationBuilder.DropColumn(
                name: "role_name",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "GooglePlaceId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attractions");

            migrationBuilder.RenameTable(
                name: "ArticleCategories",
                newName: "Article_Categories");

            migrationBuilder.RenameColumn(
                name: "log_date",
                table: "Audit_Logs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "log_data",
                table: "Audit_Logs",
                newName: "old_value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Attractions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Attractions",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Attractions",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Attractions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attractions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MainImageUrl",
                table: "Attractions",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Articles",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Articles",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Articles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Articles",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Articles",
                newName: "published_at");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                newName: "IX_Articles_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Article_Categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Article_Categories",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "Audit_Logs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "new_value",
                table: "Audit_Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "record_id",
                table: "Audit_Logs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "table_name",
                table: "Audit_Logs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "longitude",
                table: "Attractions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "latitude",
                table: "Attractions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "distance_km",
                table: "Attractions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Attractions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "map_embed_link",
                table: "Attractions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Articles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "author_id",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Articles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "Articles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Article_Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article_Categories",
                table: "Article_Categories",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Role_Dashboard_Period_States",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dashboard_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dashboard_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    period_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    period_key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    period_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    period_end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dashboard_json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comparison_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_current = table.Column<bool>(type: "bit", nullable: false),
                    last_event_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_event_source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_event_ref_id = table.Column<int>(type: "int", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    closed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Dashboard_Period_States", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Article_Categories_category_id",
                table: "Articles",
                column: "category_id",
                principalTable: "Article_Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Article_Categories_category_id",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "Role_Dashboard_Period_States");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article_Categories",
                table: "Article_Categories");

            migrationBuilder.DropColumn(
                name: "action",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "new_value",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "record_id",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "table_name",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "distance_km",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "map_embed_link",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "author_id",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "thumbnail_url",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Article_Categories",
                newName: "ArticleCategories");

            migrationBuilder.RenameColumn(
                name: "old_value",
                table: "Audit_Logs",
                newName: "log_data");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Audit_Logs",
                newName: "log_date");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Attractions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Attractions",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Attractions",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Attractions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Attractions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Attractions",
                newName: "MainImageUrl");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Articles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Articles",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Articles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Articles",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "published_at",
                table: "Articles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_category_id",
                table: "Articles",
                newName: "IX_Articles_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ArticleCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ArticleCategories",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "role_name",
                table: "Audit_Logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Attractions",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Attractions",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Attractions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ArticleCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleCategories",
                table: "ArticleCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleCategories_CategoryId",
                table: "Articles",
                column: "CategoryId",
                principalTable: "ArticleCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
