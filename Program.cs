// =========================================================================
// PROGRAM.CS - CẤU HÌNH ỨNG DỤNG VỚI 6 MODULE
// =========================================================================

using Microsoft.EntityFrameworkCore;
using KS_N5.API.Data;
using KS_N5.API.Services;
using KS_N5.API.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. THÊM CÁC SERVICES VÀO CONTAINER
// ==========================================

// Cấu hình Controller
builder.Services.AddControllers();

// Cấu hình Swagger (để test API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS (cho phép frontend gọi API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Cấu hình Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Services cho 6 Module
builder.Services.AddScoped<IBookingService, BookingService>(); // Module 2
builder.Services.AddScoped<ICMSService, CMSService>(); // Module 1
builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>(); // Module 3
builder.Services.AddScoped<IReceptionService, ReceptionService>(); // Module 4
builder.Services.AddScoped<IPaymentService, PaymentService>(); // Module 5
builder.Services.AddScoped<IHRRBACService, HRRBACService>(); // Module 6

// ==========================================
// 2. BUILD APP VÀ CẤU HÌNH PIPELINE
// ==========================================
var app = builder.Build();

// Cấu hình môi trường chạy
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Map các API Controllers
app.MapControllers();

// Chạy ứng dụng
app.Run();