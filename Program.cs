global using LossAndDamage = QuanTriKhachSanN5.Models.Loss_And_Damage;
global using OrderService = QuanTriKhachSanN5.Models.Order_Service;
global using OrderServiceDetail = QuanTriKhachSanN5.Models.Order_Service_Detail;

using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Services;
using QuanTriKhachSanN5.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi;
using QuanTriKhachSanN5.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký AuditLogFilter cho toàn bộ các Controllers
// Controllers
builder.Services.AddControllers(options => 
{
    options.Filters.Add<QuanTriKhachSanN5.Filters.AuditLogFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// JWT Service
builder.Services.AddScoped<JwtService>();

// Đăng ký bộ lọc Audit làm Scoped Service để tiêm DbContext
builder.Services.AddScoped<QuanTriKhachSanN5.Filters.AuditLogFilter>();

// Room Service
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICMSService, CMSService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IReceptionService, ReceptionService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAttractionService, AttractionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// Cloudinary & Google Maps Services
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddHttpClient<IGoogleMapsService, GoogleMapsService>();
builder.Services.AddScoped<IGoogleMapsService, GoogleMapsService>();

// CheckoutService
builder.Services.AddScoped<CheckoutService>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// ĐĂNG KÝ PHÂN QUYỀN NÂNG CAO (POLICY-BASED)
builder.Services.AddAuthorization(options =>
{
    // RBAC Policies - Require specific Permission claim
    var policies = new[] {
        "ViewBookings", "CreateBooking", "UpdateBooking", "CancelBooking",
        "ViewRooms", "ManageRooms", "UpdateRoomStatus", "ViewRoomTypes", "ManageRoomTypes",
        "ViewInventory", "UpdateInventory",
        "ViewPayments", "ManagePayments",
        "ViewAttractions", "CreateAttraction", "UpdateAttraction", "DeleteAttraction", "RestoreAttraction",
        "ManagePosts", "ViewPosts",
        "ViewReviews", "ManageReviews",
        "ViewServices", "ManageServices", "ManageOrderServices",
        "ViewLossDamages", "ManageLossDamages",
        "ManageUsers", "ManageRoles",
        "CheckInOut", "CleanRooms", "GuestViewOnly"
    };

    foreach (var policyName in policies)
    {
        options.AddPolicy(policyName, p => p.RequireClaim("Permission", policyName));
    }

// Legacy
    options.AddPolicy("MANAGE_ROOMS", policy => policy.RequireClaim("Permission", "MANAGE_ROOMS"));
    
    // NEW: AdminOnly policy for HR controllers
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Permission", "ManageRoles"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Management API",
        Version = "v1"
    });

    // Ngăn lỗi trùng lặp tên Model (ví dụ: BookingDetail) và xung đột định tuyến
    c.CustomSchemaIds(type => type.FullName);
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Vui lòng CHỈ dán chuỗi Token của bạn vào ô bên dưới (KHÔNG cần gõ chữ Bearer).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("✅ KẾT NỐI DATABASE THÀNH CÔNG!");
            Console.WriteLine($"   Server: {dbContext.Database.GetDbConnection().DataSource}");
            Console.WriteLine($"   Database: {dbContext.Database.GetDbConnection().Database}");
            
            // Kiểm tra đọc dữ liệu từ bảng Users
            var userCount = dbContext.Users.Count();
            Console.WriteLine($"   Số lượng Users: {userCount}");
        }
        else
        {
            Console.WriteLine("❌ KHÔNG THỂ KẾT NỐI DATABASE!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ LỖI KẾT NỐI: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"   Chi tiết: {ex.InnerException.Message}");
        }
    }
}

    // ==========================================
    // TỰ ĐỘNG SEED ROLES/PERMISSIONS TRƯỚC → USERS SAU (FIX CRASH)
    // ==========================================
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            // 1. FIRST: Seed Roles & Permissions (required for user assignment)
            AuthSeedData.SeedRolesAndPermissions(context);

            // 2. THEN: Create test users + assign roles (now safe)
            if (!context.Users.Any(u => u.Email == "admin@test.com"))
            {
                context.Users.AddRange(
                    new QuanTriKhachSanN5.Models.User { Username = "Admin", Email = "admin@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                    new QuanTriKhachSanN5.Models.User { Username = "Guest", Email = "guest@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                    new QuanTriKhachSanN5.Models.User { Username = "Receptionist", Email = "receptionist@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                    new QuanTriKhachSanN5.Models.User { Username = "Housekeeping", Email = "housekeeping@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow }
                );
                context.SaveChanges();

                // Safe assign roles (Roles now exist)
                var adminRole = context.Roles.First(r => r.Name == "Admin");
                var guestRole = context.Roles.First(r => r.Name == "Guest");
                var receptionistRole = context.Roles.First(r => r.Name == "Receptionist");
                var housekeepingRole = context.Roles.First(r => r.Name == "Housekeeping");

                var adminUser = context.Users.First(u => u.Email == "admin@test.com");
                var guestUser = context.Users.First(u => u.Email == "guest@test.com");
                var receptionistUser = context.Users.First(u => u.Email == "receptionist@test.com");
                var housekeepingUser = context.Users.First(u => u.Email == "housekeeping@test.com");

                context.UserRoles.AddRange(
                    new QuanTriKhachSanN5.Models.User_Role { UserId = adminUser.Id, RoleId = adminRole.Id },
                    new QuanTriKhachSanN5.Models.User_Role { UserId = guestUser.Id, RoleId = guestRole.Id },
                    new QuanTriKhachSanN5.Models.User_Role { UserId = receptionistUser.Id, RoleId = receptionistRole.Id },
                    new QuanTriKhachSanN5.Models.User_Role { UserId = housekeepingUser.Id, RoleId = housekeepingRole.Id }
                );
                context.SaveChanges();

                Console.WriteLine("✅ Đã tạo 4 test users + assigned roles!");
            }

            // 3. Seed other data
            SeedData.Initialize(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Seed data error: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"   Detail: {ex.InnerException.Message}");
        }
    }

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new { 
                error = "Internal Server Error", 
                message = exception?.Message ?? "Unknown error",
                stackTrace = exception?.StackTrace 
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        });
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
