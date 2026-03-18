using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Services;
using QuanTriKhachSanN5.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// JWT Service
builder.Services.AddScoped<JwtService>();

// Room Service
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>();

// Comment out other services to avoid build errors
// builder.Services.AddScoped<IBookingService, BookingService>();
// builder.Services.AddScoped<ICMSService, CMSService>();
// builder.Services.AddScoped<IHRRBACService, HRRBACService>();
// builder.Services.AddScoped<IPaymentService, PaymentService>();
// builder.Services.AddScoped<IReceptionService, ReceptionService>();
// builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>();

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

// Comment out Swagger to avoid build errors
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Hotel Management API",
//         Version = "v1"
//     });

//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Description = "JWT Authorization header. Example: Bearer {token}",
//         Name = "Authorization",
//         In = ParameterLocation.Header,
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer"
//     });

//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new string[] {}
//         }
//     });
// });

var app = builder.Build();

// Comment out Swagger
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

// IMPORTANT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();