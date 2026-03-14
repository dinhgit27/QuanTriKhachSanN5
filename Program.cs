using Microsoft.EntityFrameworkCore;
using KS_N5.Models;   // Đổi namespace nếu thư mục Models của bạn tên khác
using KS_N5.Services; // Nơi chứa CheckoutService

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. THÊM CÁC SERVICES VÀO CONTAINER
// ==========================================

// Cấu hình Controller
builder.Services.AddControllers();

// Cấu hình Swagger (để test API trên trình duyệt)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình Database Context (Lấy chuỗi kết nối từ appsettings.json)
builder.Services.AddDbContext<QlyKhachSanContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Service của bạn (Code mình vừa đưa ở trên)
builder.Services.AddHttpClient<CheckoutService>();
builder.Services.AddScoped<CheckoutService>();

// ==========================================
// 2. BUILD APP VÀ CẤU HÌNH PIPELINE
// ==========================================
var app = builder.Build();

// Cấu hình môi trường chạy (Mở Swagger khi đang code)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map các API Controllers
app.MapControllers();

// Chạy ứng dụng
app.Run();