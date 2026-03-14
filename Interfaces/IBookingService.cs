using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.API.DTOs;
using HotelManagement.API.DTOs.Room;
using HotelManagement.API.Models;

namespace HotelManagement.API.Interfaces
{
    public interface IBookingService
    {
        // 1. Dành cho Guest (Website)
        Task<ApiResponse<List<AvailableRoomTypeResponseDTO>>> SearchAvailableRoomTypesAsync(SearchRoomRequestDTO request);
        
        // 2. Dành cho Receptionist (ERP)
        Task<ApiResponse<List<Room>>> GetAvailablePhysicalRoomsAsync(int roomTypeId, DateTime checkIn, DateTime checkOut);
    }
}