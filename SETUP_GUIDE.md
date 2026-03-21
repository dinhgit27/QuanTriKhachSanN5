# 📋 Hướng Dẫn Khởi Chạy Hotel Management API

## ✅ Hoàn Thành

### 1. **Cấu Trúc Database**
- ✅ Migrations đã được tạo và áp dụng
- ✅ Tables: RoomTypes, Rooms, Categories, Posts, Attractions, AttractionImages
- ✅ Soft delete fields (IsDeleted, DeletedAt) đã được thêm
- ✅ Google Maps fields (GooglePlaceId, Latitude, Longitude) đã được thêm
- ✅ Cloudinary image storage fields (MainImageUrl, AttractionImages table)

### 2. **Seed Data Class**
- ✅ File: `Data/SeedData.cs` - Chứa 40+ dòng dữ liệu mẫu
  - 4 loại phòng (RoomType)
  - 20 phòng (Rooms)
  - 3 danh mục (Categories)
  - 3 bài viết (Posts)
  - 5 điểm du lịch (Attractions) với hình ảnh

### 3. **SQL Seed Script**
- ✅ File: `Data/SeedData.sql` - Script SQL để insert dữ liệu trực tiếp

## ⚠️ Vấn Đề Hiện Tại

Windows Application Control Policy đang chặn việc chạy DLL file từ Downloads folder.

## 🔧 Giải Pháp

### **Cách 1: Di chuyển Project (Khuyên Dùng)**

```powershell
# 1. Tạo thư mục Projects
mkdir C:\Projects

# 2. Copy dự án sang C:\Projects
Copy-Item -Path "C:\Users\PHI HUNG\Downloads\QuanTriKhachSanN5" -Destination "C:\Projects\" -Recurse

# 3. Chuyển đến thư mục mới
cd C:\Projects\QuanTriKhachSanN5

# 4. Clean và build lại
dotnet clean
dotnet build

# 5. Chạy ứng dụng
dotnet run
```

### **Cách 2: Disable AppLocker (Nếu Admin)**

```powershell
# Chạy PowerShell as Administrator
Get-AppLockerPolicy -Effective | Set-AppLockerPolicy -Merge
```

### **Cách 3: Unblock tay từng file**

```powershell
cd C:\Users\PHI HUNG\Downloads\QuanTriKhachSanN5
Get-ChildItem -Path . -Recurse -File | Unblock-File -Verbose
```

## 🚀 Bước Chạy Ứng Dụng

Sau khi đã giải quyết vấn đề Windows:

```powershell
# 1. Kiểm tra migrations
dotnet ef migrations list

# 2. Cập nhật database (tự động tạo từ migrations)
dotnet ef database update

# 3. Chạy ứng dụng
dotnet run
```

## 📊 Dữ Liệu Sẽ Được Khởi Chạy

Khi ứng dụng khởi động, `SeedData.Initialize()` sẽ tự động:

1. **Tạo 4 loại phòng**: Standard, Double, Family, Deluxe Suite
2. **Tạo 20 phòng**: 5 phòng cho mỗi loại
3. **Tạo 3 danh mục**: Tin tức, Sự kiện, Du lịch
4. **Tạo 3 bài viết**: Tương ứng với từng danh mục
5. **Tạo 4 tài khoản test**:
   - `admin@test.com` / `123456` - Admin
   - `user@test.com` / `123456` - User
   - `receptionist@test.com` / `123456` - Receptionist
   - `housekeeping@test.com` / `123456` - Housekeeping

6. **Tạo 5 điểm du lịch** (Attractions) với Google Maps coordinates:
   - Tháp Trầm Hương (12.2427, 109.2032)
   - Bãi biển Nha Trang (12.2485, 109.1800)
   - Núi Niêm (12.3600, 109.1200)
   - Lâu đài nước Tràm Huơng (12.2100, 109.1900)
   - Nhà thờ Đá (12.2347, 109.1956)

7. **Tạo 10 hình ảnh** (AttractionImages): Mỗi điểm du lịch có 2 hình ảnh

## 🔑 Test Tài Khoản

```
Admin Login:
  Email: admin@test.com
  Password: 123456
  Role: Admin
  
Receptionist Login:
  Email: receptionist@test.com
  Password: 123456
  Role: Receptionist
```

## 📍 Endpoints Chính

### Attractions API
- `GET /api/attractions` - Lấy danh sách điểm du lịch
- `GET /api/attractions/{id}` - Chi tiết điểm du lịch
- `POST /api/attractions` - Tạo mới (Admin/Receptionist)
- `GET /api/attractions/nearby/{lat}/{lng}` - Tìm gần đó
- `GET /api/attractions/search/{query}` - Tìm kiếm

### Authentication
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/register` - Đăng ký

## 📝 File Được Tạo/Sửa

1. **Data/SeedData.cs** - ✅ Tạo mới
2. **Data/SeedData.sql** - ✅ Tạo mới
3. **Program.cs** - ✅ Sửa (thêm SeedData.Initialize call)
4. **Migrations/20260321064508_AddAttractionsEnhancements.cs** - ✅ Tạo
5. **appsettings.json** - ✅ Sửa (thêm Cloudinary, GoogleMaps config)

## 🎯 Tiếp Theo

Sau khi ứng dụng chạy thành công:

1. Truy cập Swagger UI: `https://localhost:5001/swagger`
2. Đăng nhập bằng admin account
3. Test các API endpoints
4. Xem dữ liệu mẫu trong database

## ❓ Troubleshooting

**Vẫn có lỗi "Application Control policy blocked"?**
- Thử right-click vào project folder → Properties → Unblock (nếu có)
- Hoặc chạy: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

**Database chưa được tạo?**
- Chạy: `dotnet ef database drop --force`
- Rồi: `dotnet ef database update`

**Seed data chưa chạy?**
- Check connection string trong `appsettings.json`
- Verify database connection: Run → xem console output
