using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs; // Nhớ check namespace này
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.DTOs;

namespace QuanTriKhachSanN5.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _context;

        public AmenityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH KÈM LOGIC TÍNH TOÁN (TRẢ VỀ DTO)
        public async Task<List<AmenityDto>> GetAllAmenitiesAsync()
        {
            return await _context.Amenities
                .Select(a => new AmenityDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = a.Category,
                    Unit = a.Unit,
                    ImageUrl = a.ImageUrl,
                    ImportPrice = a.ImportPrice,
                    TotalQuantity = a.TotalQuantity,
                    
                    // Tính số lượng đã cấp dựa trên thực tế các phòng
                    IssuedQuantity = _context.RoomInventories
                        .Where(ri => ri.AmenityId == a.Id)
                        .Sum(ri => (int?)ri.Quantity) ?? 0,

                    // Tính số lượng khả dụng trong kho
                    AvailableQuantity = a.TotalQuantity - (_context.RoomInventories
                        .Where(ri => ri.AmenityId == a.Id)
                        .Sum(ri => (int?)ri.Quantity) ?? 0)
                })
                .ToListAsync();
        }

        public async Task<Amenity> GetAmenityByIdAsync(int id)
        {
            return await _context.Amenities.FindAsync(id);
        }

        public async Task<Amenity> CreateAmenityAsync(Amenity amenity)
        {
            _context.Amenities.Add(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task UpdateAmenityAsync(Amenity amenity)
        {
            // Khi update, ta update model gốc Amenity
            _context.Amenities.Update(amenity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAmenityAsync(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                _context.Amenities.Remove(amenity);
                await _context.SaveChangesAsync();
            }
        }

        // HÀM NHẬP KHO: Tăng tổng sở hữu của khách sạn
        public async Task ImportStockAsync(int id, int addedQuantity)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                amenity.TotalQuantity += addedQuantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}