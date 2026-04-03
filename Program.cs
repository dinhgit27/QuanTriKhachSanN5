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

// ============================================================
// 1. CẤU HÌNH HỆ THỐNG (SERVICES)
// ============================================================

builder.Services.AddCors(options =>
{
<<<<<<< HEAD
    options.AddPolicy(
        "AllowReactApp",
        policy =>
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
    );
=======
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
>>>>>>> origin/dinh_nguyen
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

<<<<<<< HEAD
builder.Services.AddHttpClient<CheckoutService>();

// JWT Service
builder.Services.AddScoped<JwtService>();

// Đăng ký bộ lọc Audit làm Scoped Service để tiêm DbContext
builder.Services.AddScoped<QuanTriKhachSanN5.Filters.AuditLogFilter>();

// Room Service
builder.Services.AddScoped<IRoomService, RoomService>();
=======
// --- DEPENDENCY INJECTION ---
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<ILossAndDamageService, LossAndDamageService>();
>>>>>>> origin/dinh_nguyen
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
builder.Services.AddScoped<QuanTriKhachSanN5.Filters.AuditLogFilter>();

// --- AUTHENTICATION & AUTHORIZATION ---
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
            ),
            RoleClaimType = ClaimTypes.Role,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MANAGE_ROOMS", policy => policy.RequireClaim("Permission", "MANAGE_ROOMS"));
<<<<<<< HEAD
    options.AddPolicy("VIEW_ROOMS", policy => policy.RequireClaim("Permission", "VIEW_ROOMS"));
    options.AddPolicy(
        "MANAGE_ROOMTYPES",
        policy => policy.RequireClaim("Permission", "MANAGE_ROOMTYPES")
    );
    options.AddPolicy(
        "VIEW_ROOMTYPES",
        policy => policy.RequireClaim("Permission", "VIEW_ROOMTYPES")
    );
    options.AddPolicy(
        "MANAGE_BOOKINGS",
        policy => policy.RequireClaim("Permission", "MANAGE_BOOKINGS")
    );
=======
>>>>>>> origin/dinh_nguyen
    options.AddPolicy("MANAGE_USERS", policy => policy.RequireClaim("Permission", "MANAGE_USERS"));
    options.AddPolicy("MANAGE_BOOKINGS", policy => policy.RequireClaim("Permission", "MANAGE_BOOKINGS"));
});

// --- SWAGGER (BẢN RÚT GỌN ĐỂ VƯỢT ẢI BUILD) ---
builder.Services.AddEndpointsApiExplorer();
<<<<<<< HEAD
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Management API", Version = "v1" });

    // Ngăn lỗi trùng lặp tên Model (ví dụ: BookingDetail) và xung đột định tuyến
    c.CustomSchemaIds(type => type.FullName);
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization. Vui lòng CHỈ dán chuỗi Token của bạn vào ô bên dưới (KHÔNG cần gõ chữ Bearer).",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        }
    );

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>(),
    });
});

// Thêm đoạn này để cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "http://localhost:5174") // Cho phép React ở cổng 5173 hoặc 5174 gọi vào
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    );
});

var app = builder.Build();

// ==========================================
// TỰ ĐỘNG TẠO 4 TÀI KHOẢN TEST (SEED DATA)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
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
            Console.WriteLine("Đã tạo thành công 4 tài khoản test!");
        }

        if (!context.Users.Any(u => u.Email == "admin@hotel.com"))
        {
            var admin = new User
            {
                FullName = "Admin",
                Email = "admin@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            };

            var user = new User
            {
                FullName = "Le Hoang Tuan",
                Email = "leetuan0919@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            };

            var receptionist = new User
            {
                FullName = "Receptionist",
                Email = "receptionist@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            };

            var housekeeping = new User
            {
                FullName = "Housekeeping",
                Email = "housekeeping@hotel.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            };

            context.Users.AddRange(admin, user, receptionist, housekeeping);
            context.SaveChanges();
            var roles = context.Roles.ToList();

            context.UserRoles.AddRange(
                new User_Role
                {
                    UserId = admin.Id,
                    RoleId = roles.First(r => r.Name == "Admin").Id,
                },
                new User_Role { UserId = user.Id, RoleId = roles.First(r => r.Name == "Guest").Id },
                new User_Role
                {
                    UserId = receptionist.Id,
                    RoleId = roles.First(r => r.Name == "Receptionist").Id,
                },
                new User_Role
                {
                    UserId = housekeeping.Id,
                    RoleId = roles.First(r => r.Name == "Housekeeping").Id,
                }
            );

            context.SaveChanges();

            // =========================================================================
            // SEED PERMISSIONS & ROLE_PERMISSIONS (RBAC Full)
            // =========================================================================
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(
                    new Permission { Name = "MANAGE_ROOMS" },
                    new Permission { Name = "VIEW_ROOMS" },
                    new Permission { Name = "MANAGE_ROOMTYPES" },
                    new Permission { Name = "VIEW_ROOMTYPES" },
                    new Permission { Name = "MANAGE_BOOKINGS" },
                    new Permission { Name = "VIEW_BOOKINGS" },
                    new Permission { Name = "MANAGE_USERS" }
                );
                context.SaveChanges();
            }

            var permissions = context.Permissions.ToList();
            var adminRole = roles.First(r => r.Name == "Admin");
            var receptionistRole = roles.First(r => r.Name == "Receptionist");
            var housekeepingRole = roles.First(r => r.Name == "Housekeeping");
            var guestRole = roles.First(r => r.Name == "Guest");

            if (!context.RolePermissions.Any())
            {
                context.RolePermissions.AddRange(
                    // Admin: Full access
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_ROOMS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_ROOMTYPES").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMTYPES").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_BOOKINGS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_BOOKINGS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_USERS").Id,
                    },
                    // Receptionist: Rooms & Bookings
                    new Role_Permission
                    {
                        RoleId = receptionistRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = receptionistRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_ROOMTYPES").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = receptionistRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_BOOKINGS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = receptionistRole.Id,
                        PermissionId = permissions.First(p => p.Name == "MANAGE_BOOKINGS").Id,
                    },
                    // Housekeeping: View rooms only
                    new Role_Permission
                    {
                        RoleId = housekeepingRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMS").Id,
                    },
                    // Guest: Read-only rooms/bookings
                    new Role_Permission
                    {
                        RoleId = guestRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMS").Id,
                    },
                    new Role_Permission
                    {
                        RoleId = guestRole.Id,
                        PermissionId = permissions.First(p => p.Name == "VIEW_ROOMTYPES").Id,
                    }
                );
                context.SaveChanges();
                Console.WriteLine("✅ Seed Permissions & Role_Permissions thành công!");
            }

            Console.WriteLine("Seed RBAC thành công!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Chưa thể seed data (Có thể do Database chưa sẵn sàng): " + ex.Message);
    }
}
=======
builder.Services.AddSwaggerGen(); // Chỉ dùng mặc định, bỏ qua custom để né lỗi đỏ

var app = builder.Build();

// ============================================================
// 2. CẤU HÌNH PIPELINE (MIDDLEWARE)
// ============================================================
>>>>>>> origin/dinh_nguyen

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

<<<<<<< HEAD
app.Run();
=======
// ============================================================
// 3. SEED DATA 
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    try { SeedData(context); }
    catch (Exception ex) { Console.WriteLine($"Lỗi Seed Data: {ex.Message}"); }
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
        var admin = new User { FullName = "Admin", Email = "admin@hotel.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") };
        var receptionist = new User { FullName = "Receptionist", Email = "receptionist@hotel.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") };
        
        context.Users.AddRange(admin, receptionist);
        context.SaveChanges();

        var roles = context.Roles.ToList();
        context.UserRoles.AddRange(
            new User_Role { UserId = admin.Id, RoleId = roles.First(r => r.Name == "Admin").Id },
            new User_Role { UserId = receptionist.Id, RoleId = roles.First(r => r.Name == "Receptionist").Id }
        );
        context.SaveChanges();
        Console.WriteLine("✅ Seed tài khoản mẫu thành công!");
    }
}
>>>>>>> origin/dinh_nguyen
