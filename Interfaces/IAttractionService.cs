using QuanTriKhachSanN5.DTOs.Attraction;
using QuanTriKhachSanN5.DTOs.GoogleMaps;

namespace QuanTriKhachSanN5.Interfaces;

public interface IAttractionService
{
    Task<IEnumerable<AttractionDTO>> GetAllAsync();
    Task<AttractionDTO?> GetByIdAsync(int id);
    Task<AttractionDTO> CreateAsync(CreateAttractionDTO dto);
    Task<bool> UpdateAsync(int id, CreateAttractionDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
    Task<List<GooglePlaceDetailsDTO>> GetNearbyAttractions(double latitude, double longitude, int radiusMeters = 5000);
    Task<List<GooglePlaceDetailsDTO>> SearchAttractions(string query);
}
