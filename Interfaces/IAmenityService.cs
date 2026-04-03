using System.Collections.Generic;
using System.Threading.Tasks;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.DTOs;

namespace QuanTriKhachSanN5.Interfaces
{
    public interface IAmenityService
    {
        Task<List<AmenityDto>> GetAllAmenitiesAsync(); 

        Task<Amenity> GetAmenityByIdAsync(int id);
        Task<Amenity> CreateAmenityAsync(Amenity amenity);
        Task UpdateAmenityAsync(Amenity amenity);
        Task DeleteAmenityAsync(int id);
        Task ImportStockAsync(int id, int addedQuantity);
    }
}