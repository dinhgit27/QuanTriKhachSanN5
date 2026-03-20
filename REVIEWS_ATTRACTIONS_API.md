# Hotel Management API - Hoàn thiện Attractions & Reviews

## ✅ **Hoàn thành ngày 19/03/2026**

### 1. **Attractions API** - Địa điểm Du lịch

#### Endpoints:
```
GET    /api/attractions              - Lấy tất cả địa điểm
GET    /api/attractions/{id}         - Lấy chi tiết địa điểm
POST   /api/attractions              - Tạo địa điểm (Admin/Staff)
PUT    /api/attractions/{id}         - Cập nhật địa điểm
DELETE /api/attractions/{id}         - Xóa địa điểm
```

#### Ví dụ:

**Lấy tất cả địa điểm:**
```bash
GET /api/attractions
```
Response:
```json
[
  {
    "id": 1,
    "name": "Bãi biển Nha Trang",
    "description": "Bãi biển nổi tiếng với cát trắng",
    "location": "Nha Trang, Khánh Hòa",
    "createdAt": "2026-03-19T10:00:00Z"
  }
]
```

**Tạo địa điểm mới:**
```bash
POST /api/attractions
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Bãi biển Nha Trang",
  "description": "Bãi biển nổi tiếng với cát trắng và nước trong xanh",
  "location": "Nha Trang, Khánh Hòa"
}
```
Response: 201 Created

---

### 2. **Reviews API** - Comment & Rating Phòng

Khách hàng có thể đăng comment và rating cho loại phòng mà họ đã ở.

#### Endpoints:
```
GET    /api/reviews                              - Lấy tất cả reviews (verified only)
GET    /api/reviews/roomtype/{roomTypeId}       - Lấy reviews theo loại phòng
GET    /api/reviews/roomtype/{roomTypeId}/average-rating - Rating trung bình
GET    /api/reviews/my-reviews                  - Lấy reviews của user hiện tại (Auth required)
GET    /api/reviews/{id}                        - Lấy chi tiết review
POST   /api/reviews                             - Tạo review mới (Auth required)
PUT    /api/reviews/{id}                        - Cập nhật review (Auth required)
DELETE /api/reviews/{id}                        - Xóa review (Auth required)
```

#### Ví dụ:

**Lấy reviews theo loại phòng:**
```bash
GET /api/reviews/roomtype/1
```
Response:
```json
[
  {
    "id": 1,
    "userId": 5,
    "username": "john_hotel",
    "roomTypeId": 1,
    "roomTypeName": "Deluxe Suite",
    "rating": 5,
    "comment": "Phòng rất sạch, thoải mái và có tầm nhìn đẹp!",
    "cleanliness": 5,
    "comfort": 5,
    "serviceQuality": 4,
    "valueForMoney": 5,
    "createdAt": "2026-03-19T10:00:00Z",
    "isVerified": true
  }
]
```

**Lấy rating trung bình loại phòng:**
```bash
GET /api/reviews/roomtype/1/average-rating
```
Response:
```json
{
  "roomTypeId": 1,
  "averageRating": 4.6
}
```

**Tạo review (Khách hàng):**
```bash
POST /api/reviews
Authorization: Bearer {token}
Content-Type: application/json

{
  "roomTypeId": 1,
  "bookingId": 1,
  "rating": 5,
  "comment": "Phòng rất sạch, thoải mái và có tầm nhìn đẹp. Nhân viên rất thân thiện!",
  "cleanliness": 5,
  "comfort": 5,
  "serviceQuality": 4,
  "valueForMoney": 5
}
```
Response: 201 Created
```json
{
  "id": 1,
  "userId": 5,
  "username": "john_hotel",
  "roomTypeId": 1,
  "roomTypeName": "Deluxe Suite",
  "rating": 5,
  "comment": "Phòng rất sạch, thoải mái...",
  "cleanliness": 5,
  "comfort": 5,
  "serviceQuality": 4,
  "valueForMoney": 5,
  "createdAt": "2026-03-19T10:30:00Z",
  "isVerified": true
}
```

**Cập nhật review:**
```bash
PUT /api/reviews/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "rating": 4,
  "comment": "Phòng rất sạch. Chỉ có tiếng ồn từ đường phố.",
  "comfort": 4
}
```
Response: 204 No Content

**Xóa review:**
```bash
DELETE /api/reviews/1
Authorization: Bearer {token}
```
Response: 204 No Content

---

## 📊 **Model Structure**

### Review Model
```csharp
public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }              // User who reviewed
    public int RoomTypeId { get; set; }          // Room type reviewed
    public int? BookingId { get; set; }          // Booking reference
    public int Rating { get; set; }              // 1-5 stars
    public string Comment { get; set; }          // Review comment
    
    // Detailed ratings
    public int? Cleanliness { get; set; }        // 1-5 stars
    public int? Comfort { get; set; }            // 1-5 stars
    public int? ServiceQuality { get; set; }     // 1-5 stars
    public int? ValueForMoney { get; set; }      // 1-5 stars
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsVerified { get; set; }         // Verified guest (ở qua phòng)
}
```

---

## 🔒 **Authentication & Authorization**

### Public Endpoints (No token required):
- `GET /api/attractions` - Xem địa điểm
- `GET /api/attractions/{id}`
- `GET /api/reviews` - Xem reviews
- `GET /api/reviews/roomtype/{id}`
- `GET /api/reviews/roomtype/{id}/average-rating`
- `GET /api/reviews/{id}`

### Protected Endpoints (Token required):
- `POST /api/attractions` - Admin/Staff only
- `PUT /api/attractions/{id}` - Admin/Staff only
- `DELETE /api/attractions/{id}` - Admin/Staff only
- `POST /api/reviews` - Authenticated users
- `GET /api/reviews/my-reviews` - Own reviews only
- `PUT /api/reviews/{id}` - Own review only
- `DELETE /api/reviews/{id}` - Own review only

---

## 💾 **Database Changes**

### Migrations Required:
```bash
dotnet ef migrations add AddReviewModel
dotnet ef database update
```

### DbContext Update:
```csharp
public DbSet<Review> Reviews { get; set; }
public DbSet<Attraction> Attractions { get; set; }
```

---

## 🧪 **Testing**

### Using REST Client (VS Code):
1. Install "REST Client" extension
2. Open `test_requests.http` file
3. Click "Send Request" on each request

### Test Review Flow:
1. **Login** → Get JWT token
2. **Create Review** → POST /api/reviews
3. **Get Reviews** → GET /api/reviews/roomtype/1
4. **Get Average Rating** → GET /api/reviews/roomtype/1/average-rating
5. **Update Review** → PUT /api/reviews/{id}
6. **Delete Review** → DELETE /api/reviews/{id}

---

## 📋 **Service Methods**

### ReviewService
```csharp
// Get all verified reviews
Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()

// Get reviews by room type
Task<IEnumerable<ReviewDTO>> GetReviewsByRoomTypeAsync(int roomTypeId)

// Get user's reviews
Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(int userId)

// Get single review
Task<ReviewDTO?> GetReviewByIdAsync(int id)

// Create review (auto-verify if user stayed in room)
Task<ReviewDTO> CreateReviewAsync(int userId, CreateReviewDTO dto)

// Update review (only owner)
Task<bool> UpdateReviewAsync(int id, int userId, UpdateReviewDTO dto)

// Delete review (only owner)
Task<bool> DeleteReviewAsync(int id, int userId)

// Get average rating by room type
Task<double> GetAverageRatingByRoomTypeAsync(int roomTypeId)
```

### AttractionService
```csharp
// Get all attractions
Task<IEnumerable<AttractionDTO>> GetAllAsync()

// Get single attraction
Task<AttractionDTO?> GetByIdAsync(int id)

// Create attraction
Task<AttractionDTO> CreateAsync(CreateAttractionDTO dto)

// Update attraction
Task<bool> UpdateAsync(int id, CreateAttractionDTO dto)

// Delete attraction
Task<bool> DeleteAsync(int id)
```

---

## 🚀 **Status Summary**

| Feature | Status | Endpoints |
|---------|--------|-----------|
| Attractions API | ✅ Complete | 5/5 |
| Reviews API | ✅ Complete | 8/8 |
| **Total** | **✅ 100%** | **13/13** |

---

## 📚 **Files Created/Modified**

### New Files Created:
- `Models/Review.cs` - Review model
- `DTOs/Review/ReviewDTO.cs` - Review DTO
- `DTOs/Review/CreateReviewDTO.cs` - Create DTO
- `DTOs/Review/UpdateReviewDTO.cs` - Update DTO
- `Interfaces/IReviewService.cs` - Service interface
- `Services/ReviewService.cs` - Service implementation
- `Controllers/ReviewsController.cs` - Controller
- `test_requests.http` - API test requests

### Modified Files:
- `Program.cs` - Added ReviewService & AttractionService registrations
- `Controllers/AttractionsController.cs` - Already complete (confirmed)

---

## ⚡ **Next Steps**

1. **Run migrations:**
   ```bash
   dotnet ef migrations add AddReviewModel
   dotnet ef database update
   ```

2. **Test API endpoints:**
   - Open `test_requests.http` in VS Code
   - Install REST Client extension
   - Send requests

3. **Optional enhancements:**
   - Add pagination to reviews
   - Add review filtering (date range, rating range)
   - Add helpful/unhelpful votes on reviews
   - Add moderation for reviews
   - Add email notifications for new reviews

---

## 📞 **Support**

For more details, refer to:
- `API_ENDPOINTS.md` - Full API documentation
- `API_STATUS.md` - Status report
- `test_requests.http` - Test examples
