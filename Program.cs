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
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using QuanTriKhachSanN5.Models;
using Microsoft.Extensions.Logging;  // Added for logger

var builder = WebApplication.CreateBuilder(args);

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

// Services...
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<QuanTriKhachSanN5.Filters.AuditLogFilter>();
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
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddHttpClient<IGoogleMapsService, GoogleMapsService>();
builder.Services.AddScoped<IGoogleMapsService, GoogleMapsService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<ILossAndDamageService, LossAndDamageService>();
builder.Services.AddScoped<IServiceService, ServiceService>();


// Authentication with logging
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("JWT Authentication failed: {Reason}", context.Exception?.Message);
            return Task.CompletedTask;
        }
    };
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
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

    options.AddPolicy("MANAGE_ROOMS", policy => policy.RequireClaim("Permission", "MANAGE_ROOMS"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Permission", "ManageRoles"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Management API", Version = "v1" });
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

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// DB check & seed
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
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database connection failed");
        Console.WriteLine($"❌ LỖI KẾT NỐI: {ex.Message}");
    }
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        AuthSeedData.SeedRolesAndPermissions(context);

        if (!context.Users.Any(u => u.Email == "admin@test.com"))
        {
            // Add test users...
            context.Users.AddRange(
                new QuanTriKhachSanN5.Models.User { Username = "Admin", Email = "admin@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                new QuanTriKhachSanN5.Models.User { Username = "Guest", Email = "guest@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                new QuanTriKhachSanN5.Models.User { Username = "Receptionist", Email = "receptionist@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow },
                new QuanTriKhachSanN5.Models.User { Username = "Housekeeping", Email = "housekeeping@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), CreatedAt = DateTime.UtcNow }
            );
            context.SaveChanges();

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var guestRole = context.Roles.FirstOrDefault(r => r.Name == "Guest") ?? throw new InvalidOperationException("Guest role not found");
            var receptionistRole = context.Roles.FirstOrDefault(r => r.Name == "Receptionist") ?? throw new InvalidOperationException("Receptionist role not found");
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

        SeedData.Initialize(context);
    }
catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed data failed");
        Console.WriteLine($"❌ Seed data error: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
