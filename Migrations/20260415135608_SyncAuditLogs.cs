using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanTriKhachSanN5.Migrations
{
    /// <inheritdoc />
    public partial class SyncAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_GuestId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServiceDetails_OrderServices_OrderServiceId",
                table: "OrderServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServiceDetails_Services_ServiceId",
                table: "OrderServiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Bookings_BookingId",
                table: "OrderServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_RoomTypes_RoomTypeId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInventories_Amenities_AmenityId",
                table: "RoomInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomInventories_Rooms_RoomId",
                table: "RoomInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategories_CategoryId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "LossAndDamages");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCategories",
                table: "ServiceCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTypes",
                table: "RoomTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomInventories",
                table: "RoomInventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderServices",
                table: "OrderServices");

            migrationBuilder.DropIndex(
                name: "IX_OrderServices_BookingId",
                table: "OrderServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderServiceDetails",
                table: "OrderServiceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DamageFee",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RoomTotalCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ServicesCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "VoucherDiscount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CheckInDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CheckOutDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoomInventories");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "AuditLogs");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "User_Role");

            migrationBuilder.RenameTable(
                name: "ServiceCategories",
                newName: "Service_Categories");

            migrationBuilder.RenameTable(
                name: "RoomTypes",
                newName: "Room_Types");

            migrationBuilder.RenameTable(
                name: "RoomInventories",
                newName: "Room_Inventory");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "Role_Permission");

            migrationBuilder.RenameTable(
                name: "OrderServices",
                newName: "Order_Services");

            migrationBuilder.RenameTable(
                name: "OrderServiceDetails",
                newName: "Order_Service_Details");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "Audit_Logs");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Services",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                newName: "IX_Services_category_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Rooms",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "Rooms",
                newName: "room_type_id");

            migrationBuilder.RenameColumn(
                name: "RoomNumber",
                table: "Rooms",
                newName: "room_number");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                newName: "IX_Rooms_room_type_id");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "RoomImages",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "RoomImages",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "RoomImages",
                newName: "room_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImages_RoomId",
                table: "RoomImages",
                newName: "IX_RoomImages_room_type_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "Payments",
                newName: "invoice_id");

            migrationBuilder.RenameColumn(
                name: "AmountPaid",
                table: "Payments",
                newName: "amount_paid");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Invoices",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Invoices",
                newName: "booking_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bookings",
                newName: "status");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserId",
                table: "User_Role",
                newName: "IX_User_Role_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "User_Role",
                newName: "IX_User_Role_RoleId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Room_Types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Room_Types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Room_Types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CapacityChildren",
                table: "Room_Types",
                newName: "capacity_children");

            migrationBuilder.RenameColumn(
                name: "CapacityAdults",
                table: "Room_Types",
                newName: "capacity_adults");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "Room_Types",
                newName: "base_price");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Room_Inventory",
                newName: "room_id");

            migrationBuilder.RenameColumn(
                name: "AmenityId",
                table: "Room_Inventory",
                newName: "EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomInventories_RoomId",
                table: "Room_Inventory",
                newName: "IX_Room_Inventory_room_id");

            migrationBuilder.RenameIndex(
                name: "IX_RoomInventories_AmenityId",
                table: "Room_Inventory",
                newName: "IX_Room_Inventory_EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "Role_Permission",
                newName: "IX_Role_Permission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "Role_Permission",
                newName: "IX_Role_Permission_PermissionId");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Order_Services",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Order_Services",
                newName: "order_date");

            migrationBuilder.RenameColumn(
                name: "BookingDetailId",
                table: "Order_Services",
                newName: "booking_detail_id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Order_Service_Details",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Order_Service_Details",
                newName: "service_id");

            migrationBuilder.RenameColumn(
                name: "OrderServiceId",
                table: "Order_Service_Details",
                newName: "order_service_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderServiceDetails_ServiceId",
                table: "Order_Service_Details",
                newName: "IX_Order_Service_Details_service_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderServiceDetails_OrderServiceId",
                table: "Order_Service_Details",
                newName: "IX_Order_Service_Details_order_service_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Audit_Logs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Audit_Logs",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Audit_Logs",
                newName: "log_date");

            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "Audit_Logs",
                newName: "log_data");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_UserId",
                table: "Audit_Logs",
                newName: "IX_Audit_Logs_user_id");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "cleaning_status",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "extension_number",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "floor",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "RoomImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_method",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "invoice_id",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_date",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_code",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<decimal>(
                name: "discount_amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "final_total",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_room_amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_service_amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "booking_code",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guest_email",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guest_name",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guest_phone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "voucher_id",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "compensation_price",
                table: "Amenities",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon_url",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "import_price",
                table: "Amenities",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "supplier",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_quantity",
                table: "Amenities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Room_Types",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "bed_type",
                table: "Room_Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "Room_Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Room_Types",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "size_sqm",
                table: "Room_Types",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "Room_Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "view_type",
                table: "Room_Types",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Room_Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Room_Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                table: "Room_Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_if_lost",
                table: "Room_Inventory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "role_name",
                table: "Audit_Logs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Role",
                table: "User_Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service_Categories",
                table: "Service_Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room_Types",
                table: "Room_Types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room_Inventory",
                table: "Room_Inventory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role_Permission",
                table: "Role_Permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Services",
                table: "Order_Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Service_Details",
                table: "Order_Service_Details",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audit_Logs",
                table: "Audit_Logs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Booking_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    room_type_id = table.Column<int>(type: "int", nullable: true),
                    check_in_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price_per_night = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Details_Bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Details_Room_Types_room_type_id",
                        column: x => x.room_type_id,
                        principalTable: "Room_Types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booking_Details_Rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalQuantity = table.Column<int>(type: "int", nullable: true),
                    InUseQuantity = table.Column<int>(type: "int", nullable: true),
                    DamagedQuantity = table.Column<int>(type: "int", nullable: true),
                    InStockQuantity = table.Column<int>(type: "int", nullable: true),
                    LiquidatedQuantity = table.Column<int>(type: "int", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DefaultPriceIfLost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_Role_Permissions_Permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_Permissions_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_User_Roles_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Roles_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loss_And_Damages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_detail_id = table.Column<int>(type: "int", nullable: true),
                    room_inventory_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    penalty_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loss_And_Damages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loss_And_Damages_Booking_Details_booking_detail_id",
                        column: x => x.booking_detail_id,
                        principalTable: "Booking_Details",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_booking_id",
                table: "Invoices",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Details_booking_id",
                table: "Booking_Details",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Details_room_id",
                table: "Booking_Details",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Details_room_type_id",
                table: "Booking_Details",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Loss_And_Damages_booking_detail_id",
                table: "Loss_And_Damages",
                column: "booking_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permissions_permission_id",
                table: "Role_Permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_role_id",
                table: "User_Roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Bookings_booking_id",
                table: "Invoices",
                column: "booking_id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Service_Details_Order_Services_order_service_id",
                table: "Order_Service_Details",
                column: "order_service_id",
                principalTable: "Order_Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Service_Details_Services_service_id",
                table: "Order_Service_Details",
                column: "service_id",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Room_Types_RoomTypeId",
                table: "Reviews",
                column: "RoomTypeId",
                principalTable: "Room_Types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Permission_Permission_PermissionId",
                table: "Role_Permission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Permission_Role_RoleId",
                table: "Role_Permission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Inventory_Equipments_EquipmentId",
                table: "Room_Inventory",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Inventory_Rooms_room_id",
                table: "Room_Inventory",
                column: "room_id",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Room_Types_room_type_id",
                table: "RoomImages",
                column: "room_type_id",
                principalTable: "Room_Types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Room_Types_room_type_id",
                table: "Rooms",
                column: "room_type_id",
                principalTable: "Room_Types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Service_Categories_category_id",
                table: "Services",
                column: "category_id",
                principalTable: "Service_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Role_RoleId",
                table: "User_Role",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Users_UserId",
                table: "User_Role",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audit_Logs_Users_user_id",
                table: "Audit_Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Bookings_booking_id",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Service_Details_Order_Services_order_service_id",
                table: "Order_Service_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Service_Details_Services_service_id",
                table: "Order_Service_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Room_Types_RoomTypeId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Permission_Permission_PermissionId",
                table: "Role_Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Permission_Role_RoleId",
                table: "Role_Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Inventory_Equipments_EquipmentId",
                table: "Room_Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Inventory_Rooms_room_id",
                table: "Room_Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Room_Types_room_type_id",
                table: "RoomImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Room_Types_room_type_id",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Service_Categories_category_id",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Role_RoleId",
                table: "User_Role");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Users_UserId",
                table: "User_Role");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Loss_And_Damages");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Role_Permissions");

            migrationBuilder.DropTable(
                name: "User_Roles");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Booking_Details");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_booking_id",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Role",
                table: "User_Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service_Categories",
                table: "Service_Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room_Types",
                table: "Room_Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room_Inventory",
                table: "Room_Inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role_Permission",
                table: "Role_Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Services",
                table: "Order_Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Service_Details",
                table: "Order_Service_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Audit_Logs",
                table: "Audit_Logs");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "cleaning_status",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "extension_number",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "floor",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "RoomImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "payment_date",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "transaction_code",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "discount_amount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "final_total",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "tax_amount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "total_room_amount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "total_service_amount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "booking_code",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "guest_email",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "guest_name",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "guest_phone",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "voucher_id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "category",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "compensation_price",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "icon_url",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "import_price",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "supplier",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "total_quantity",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "bed_type",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "content",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "size_sqm",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "view_type",
                table: "Room_Types");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Room_Inventory");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Room_Inventory");

            migrationBuilder.DropColumn(
                name: "item_type",
                table: "Room_Inventory");

            migrationBuilder.DropColumn(
                name: "price_if_lost",
                table: "Room_Inventory");

            migrationBuilder.DropColumn(
                name: "role_name",
                table: "Audit_Logs");

            migrationBuilder.RenameTable(
                name: "User_Role",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Service_Categories",
                newName: "ServiceCategories");

            migrationBuilder.RenameTable(
                name: "Room_Types",
                newName: "RoomTypes");

            migrationBuilder.RenameTable(
                name: "Room_Inventory",
                newName: "RoomInventories");

            migrationBuilder.RenameTable(
                name: "Role_Permission",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "Order_Services",
                newName: "OrderServices");

            migrationBuilder.RenameTable(
                name: "Order_Service_Details",
                newName: "OrderServiceDetails");

            migrationBuilder.RenameTable(
                name: "Audit_Logs",
                newName: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Services",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_category_id",
                table: "Services",
                newName: "IX_Services_CategoryId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Rooms",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "room_type_id",
                table: "Rooms",
                newName: "RoomTypeId");

            migrationBuilder.RenameColumn(
                name: "room_number",
                table: "Rooms",
                newName: "RoomNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_room_type_id",
                table: "Rooms",
                newName: "IX_Rooms_RoomTypeId");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "RoomImages",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "RoomImages",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "room_type_id",
                table: "RoomImages",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImages_room_type_id",
                table: "RoomImages",
                newName: "IX_RoomImages_RoomId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "Payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "invoice_id",
                table: "Payments",
                newName: "InvoiceId");

            migrationBuilder.RenameColumn(
                name: "amount_paid",
                table: "Payments",
                newName: "AmountPaid");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Invoices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "booking_id",
                table: "Invoices",
                newName: "BookingId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_User_Role_UserId",
                table: "UserRoles",
                newName: "IX_UserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Role_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "RoomTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "RoomTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RoomTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "capacity_children",
                table: "RoomTypes",
                newName: "CapacityChildren");

            migrationBuilder.RenameColumn(
                name: "capacity_adults",
                table: "RoomTypes",
                newName: "CapacityAdults");

            migrationBuilder.RenameColumn(
                name: "base_price",
                table: "RoomTypes",
                newName: "BasePrice");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "RoomInventories",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "RoomInventories",
                newName: "AmenityId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_Inventory_room_id",
                table: "RoomInventories",
                newName: "IX_RoomInventories_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_Inventory_EquipmentId",
                table: "RoomInventories",
                newName: "IX_RoomInventories_AmenityId");

            migrationBuilder.RenameIndex(
                name: "IX_Role_Permission_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Role_Permission_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "OrderServices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "order_date",
                table: "OrderServices",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "booking_detail_id",
                table: "OrderServices",
                newName: "BookingDetailId");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "OrderServiceDetails",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "service_id",
                table: "OrderServiceDetails",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "order_service_id",
                table: "OrderServiceDetails",
                newName: "OrderServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Service_Details_service_id",
                table: "OrderServiceDetails",
                newName: "IX_OrderServiceDetails_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Service_Details_order_service_id",
                table: "OrderServiceDetails",
                newName: "IX_OrderServiceDetails_OrderServiceId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AuditLogs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AuditLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "log_date",
                table: "AuditLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "log_data",
                table: "AuditLogs",
                newName: "TableName");

            migrationBuilder.RenameIndex(
                name: "IX_Audit_Logs_user_id",
                table: "AuditLogs",
                newName: "IX_AuditLogs_UserId");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Vouchers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Vouchers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DamageFee",
                table: "Invoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RoomTotalCost",
                table: "Invoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServicesCost",
                table: "Invoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "VoucherDiscount",
                table: "Invoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Bookings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Attractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Amenities",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RoomInventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "OrderServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCategories",
                table: "ServiceCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTypes",
                table: "RoomTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomInventories",
                table: "RoomInventories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderServices",
                table: "OrderServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderServiceDetails",
                table: "OrderServiceDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LossAndDamages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    BookingDetailId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomInventoryId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossAndDamages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossAndDamages_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_BookingId",
                table: "OrderServices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingId",
                table: "BookingDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomId",
                table: "BookingDetails",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomTypeId",
                table: "BookingDetails",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LossAndDamages_BookingId",
                table: "LossAndDamages",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_GuestId",
                table: "Bookings",
                column: "GuestId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServiceDetails_OrderServices_OrderServiceId",
                table: "OrderServiceDetails",
                column: "OrderServiceId",
                principalTable: "OrderServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServiceDetails_Services_ServiceId",
                table: "OrderServiceDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Bookings_BookingId",
                table: "OrderServices",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_RoomTypes_RoomTypeId",
                table: "Reviews",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInventories_Amenities_AmenityId",
                table: "RoomInventories",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInventories_Rooms_RoomId",
                table: "RoomInventories",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
