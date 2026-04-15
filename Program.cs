using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanTriKhachSanN5.API.Services;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// ============================================================
// 1. CẤU HÌNH HỆ THỐNG (SERVICES)
// ============================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "http://localhost:5174")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder
    .Services.AddControllers(options => options.Filters.Add<QuanTriKhachSanN5.Filters.AuditLogFilter>())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// --- DEPENDENCY INJECTION ---
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<ILossAndDamageService, LossAndDamageService>();
builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IReceptionService, ReceptionService>();
builder.Services.AddScoped<IHRRBACService, HRRBACService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICMSService, CMSService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAttractionService, AttractionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<JwtService>();
// AuditLogFilter registered globally above

// --- AUTHENTICATION & AUTHORIZATION ---
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ),
            RoleClaimType = ClaimTypes.Role,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MANAGE_ROOMS", policy => policy.RequireClaim("Permission", "MANAGE_ROOMS"));
    options.AddPolicy("MANAGE_USERS", policy => policy.RequireClaim("Permission", "MANAGE_USERS"));
    options.AddPolicy(
        "MANAGE_BOOKINGS",
        policy => policy.RequireClaim("Permission", "MANAGE_BOOKINGS")
    );
});

// --- SWAGGER (BẢN RÚT GỌN ĐỂ VƯỢT ẢI BUILD) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Chỉ dùng mặc định, bỏ qua custom để né lỗi đỏ

var app = builder.Build();

// ============================================================
// 2. CẤU HÌNH PIPELINE (MIDDLEWARE)
// ============================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ============================================================
// 3. SEED DATA
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    try
    {
        SeedData(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Lỗi Seed Data: {ex.Message}");
    }
}

app.Run();

void SeedData(ApplicationDbContext context)
{
    if (!context.Roles.Any())
    {
        context.Roles.AddRange(
            new Role { Name = "Admin" },
            new Role { Name = "Guest" },
            new Role { Name = "Receptionist" },
            new Role { Name = "Housekeeping" }
        );
        context.SaveChanges();
    }

    if (!context.Users.Any(u => u.Email == "admin@hotel.com"))
    {
        var admin = new User
        {
            FullName = "Admin",
            Email = "admin@hotel.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
        };
        var receptionist = new User
        {
            FullName = "Receptionist",
            Email = "receptionist@hotel.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
        };

        context.Users.AddRange(admin, receptionist);
        context.SaveChanges();

        var roles = context.Roles.ToList();
        context.UserRoles.AddRange(
            new User_Role { UserId = admin.Id, RoleId = roles.First(r => r.Name == "Admin").Id },
            new User_Role
            {
                UserId = receptionist.Id,
                RoleId = roles.First(r => r.Name == "Receptionist").Id,
            }
        );
        context.SaveChanges();
        Console.WriteLine("✅ Seed tài khoản mẫu thành công!");
    }
}
