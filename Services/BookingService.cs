using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagement.API.Data;
using HotelManagement.API.DTOs;
using HotelManagement.API.DTOs.Room;
using HotelManagement.API.Interfaces;
using HotelManagement.API.Models;

namespace HotelManagement.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // THUẬT TOÁN 1: TÌM LOẠI PHÒNG TRỐNG CHO WEBSITE
        // =========================================================================
        public async Task<ApiResponse<List<AvailableRoomTypeResponseDTO>>> SearchAvailableRoomTypesAsync(SearchRoomRequestDTO request)
        {
            if (request.CheckInDate >= request.CheckOutDate)
            {
                return new ApiResponse<List<AvailableRoomTypeResponseDTO>>(false, "Ngày Check-out phải lớn hơn Check-in");
            }

            // Dùng LINQ để thực thi câu Query đếm phòng trống
            var availableRoomTypes = await _context.Room_Types
                .Where(rt => rt.capacity_adults >= request.Adults && rt.capacity_children >= request.Children)
                .Select(rt => new 
                {
                    RoomType = rt,
                    // Đếm tổng số phòng vật lý của loại này (Không tính phòng đang bảo trì)
                    TotalPhysicalRooms = _context.Rooms.Count(r => r.room_type_id == rt.id && r.status != "Maintenance"),
                    
                    // Đếm số phòng đã bị đặt (Công thức Overlap)
                    BookedRoomsCount = _context.Booking_Details
                        .Where(bd => bd.room_type_id == rt.id 
                                  && bd.Booking.status != "Cancelled" // Bỏ qua các booking đã hủy
                                  && bd.check_in_date < request.CheckOutDate 
                                  && bd.check_out_date > request.CheckInDate) // LOGIC OVERLAP CHÍNH LÀ ĐÂY
                        .Count()
                })
                .Where(x => (x.TotalPhysicalRooms - x.BookedRoomsCount) > 0) // Chỉ lấy loại phòng còn dư
                .Select(x => new AvailableRoomTypeResponseDTO
                {
                    RoomTypeId = x.RoomType.id,
                    Name = x.RoomType.name,
                    BasePrice = x.RoomType.base_price,
                    AvailableCount = x.TotalPhysicalRooms - x.BookedRoomsCount // Tính ra số phòng trống
                })
                .ToListAsync();

            return new ApiResponse<List<AvailableRoomTypeResponseDTO>>(true, "Tìm phòng thành công", availableRoomTypes);
        }

        // =========================================================================
        // THUẬT TOÁN 2: TÌM PHÒNG VẬT LÝ CHO LỄ TÂN (CHECK-IN)
        // =========================================================================
        public async Task<ApiResponse<List<Room>>> GetAvailablePhysicalRoomsAsync(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            // 1. Lấy danh sách ID các phòng VẬT LÝ đã bị khóa trong khoảng thời gian này
            var bookedRoomIds = await _context.Booking_Details
                .Where(bd => bd.room_type_id == roomTypeId 
                          && bd.Booking.status != "Cancelled" 
                          && bd.room_id != null // Chỉ xét những booking đã được Lễ tân gán số phòng
                          && bd.check_in_date < checkOut 
                          && bd.check_out_date > checkIn) // LOGIC OVERLAP
                .Select(bd => bd.room_id)
                .ToListAsync();

            // 2. Tìm các phòng thuộc Loại phòng này, nhưng ID KHÔNG NẰM TRONG danh sách đã đặt ở trên
            var availablePhysicalRooms = await _context.Rooms
                .Where(r => r.room_type_id == roomTypeId 
                         && r.status != "Maintenance" 
                         && !bookedRoomIds.Contains(r.id)) // Tương đương lệnh NOT IN trong SQL
                .ToListAsync();

            return new ApiResponse<List<Room>>(true, "Lấy danh sách phòng vật lý thành công", availablePhysicalRooms);
        }
    }
}