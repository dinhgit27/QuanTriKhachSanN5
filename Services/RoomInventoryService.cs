// =========================================================================
// MODULE 3: ROOM INVENTORY - SERVICE (BẢN FULL OPTION)
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class RoomInventoryService : IRoomInventoryService
    {
        private readonly ApplicationDbContext _context;

        public RoomInventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<object>> GetRoomsAsync()
        {
            // BƯỚC 1: Dùng ToListAsync() TRƯỚC để ép C# kéo hết dữ liệu thật từ SQL về RAM.
            // Điều này giúp cắt đứt mọi lỗi dịch câu lệnh của Entity Framework.
            var rooms = await _context.Rooms.Include(r => r.RoomType).ToListAsync();

            // BƯỚC 2: Sau khi có data, mới dùng Select để tạo object gọn nhẹ, cắt đứt vòng lặp JSON.
            var result = rooms
                .Select(r => new
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Status = r.Status,
                    RoomTypeName = r.RoomType != null ? r.RoomType.Name : "Chưa xác định",
                })
                .Cast<object>()
                .ToList();

            return result;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context
                .Rooms.Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRoomStatusAsync(int roomId, string status)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                room.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Amenity>> GetAmenitiesAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<List<Room_Inventory>> GetRoomInventoryAsync(int roomId)
        {
            return await _context
                .RoomInventories.Where(ri => ri.RoomId == roomId)
                .Include(ri => ri.Amenity)
                .ToListAsync();
        }

        // =========================================================
        // CÁC HÀM MỚI BỔ SUNG ĐỂ REACT KHÔNG BỊ LỖI 404/500
        // =========================================================

        public async Task<List<Room_Inventory>> GetAllInventoriesAsync()
        {
            // Lấy tất cả vật tư, nhớ Include để React lấy được tên Phòng và tên Vật tư
            return await _context
                .RoomInventories.Include(ri => ri.Room)
                .Include(ri => ri.Amenity)
                .ToListAsync();
        }

        public async Task AddRoomInventoryAsync(Room_Inventory inventory)
        {
            _context.RoomInventories.Add(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomInventoryAsync(Room_Inventory inventory)
        {
            _context.RoomInventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomInventoryAsync(int id)
        {
            var inventory = await _context.RoomInventories.FindAsync(id);
            if (inventory != null)
            {
                _context.RoomInventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRoomImageAsync(Room_Image image)
        {
            // Lưu ý: Trong file ApplicationDbContext.cs của ní phải có public DbSet<Room_Image> RoomImages { get; set; } nha
            _context.Add(image);
            await _context.SaveChangesAsync();
        }
    }
}
