using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KS_N5.API.DTOs;
using KS_N5.API.DTOs.Room;
using KS_N5.API.Models;

namespace KS_N5.API.Interfaces
{
    public interface IBookingService
    {
        // 1. Dành cho Guest (Website)
        Task<ApiResponse<List<AvailableRoomTypeResponseDTO>>> SearchAvailableRoomTypesAsync(SearchRoomRequestDTO request);
        
        // 2. Dành cho Receptionist (ERP)
        Task<ApiResponse<List<Room>>> GetAvailablePhysicalRoomsAsync(int roomTypeId, DateTime checkIn, DateTime checkOut);
    }
}