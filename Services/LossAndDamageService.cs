using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class LossAndDamageService : ILossAndDamageService
    {
        private readonly ApplicationDbContext _context;

        public LossAndDamageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LossAndDamage>> GetAllLossAndDamagesAsync()
        {
            // ĐÃ FIX: Đổi ThenInclude(ri => ri.Amenity) thành ThenInclude(ri => ri.Equipment)
            return await _context.LossAndDamages
                .Include(ld => ld.RoomInventory)
                    .ThenInclude(ri => ri.Equipment)
                .ToListAsync();
        }

        public async Task<LossAndDamage> GetLossAndDamageByIdAsync(int id)
        {
            return await _context.LossAndDamages.FindAsync(id);
        }

        public async Task<LossAndDamage> CreateLossAndDamageAsync(LossAndDamage report)
        {
            if (string.IsNullOrEmpty(report.Status))
            {
                report.Status = "Chưa đền bù";
            }

            _context.LossAndDamages.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<LossAndDamage> UpdateLossAndDamageAsync(int id, LossAndDamage model)
        {
            var data = await _context.LossAndDamages.FindAsync(id);
            if (data == null)
                return null;

            data.BookingDetailId = model.BookingDetailId;
            data.RoomInventoryId = model.RoomInventoryId;
            data.Quantity = model.Quantity;
            data.PenaltyAmount = model.PenaltyAmount;
            data.Description = model.Description;
            data.ImageUrl = model.ImageUrl;
            data.Status = model.Status; // Cập nhật thêm status nếu cần

            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var data = await _context.LossAndDamages.FindAsync(id);
            if (data == null)
                return false;

            data.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
