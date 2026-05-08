// =========================================================================
// MODULE 1: TRẢI NGHIỆM DU LỊCH & CMS - MODELS
// =========================================================================

namespace QuanTriKhachSanN5.Models
{
    // Bảng Articles: Bài viết blog, thông tin phòng
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public Article_Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Bảng Article_Categories: Danh mục bài viết
    public class Article_Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }

    // Bảng Attractions: Địa điểm tham quan lân cận
    // public class Attraction
    // {
    //     public int Id { get; set; }
    //     public string Name { get; set; }
    //     public string Description { get; set; }
    //     public string Location { get; set; }
    // }

    // Bảng Reviews: Đánh giá phòng của khách
    // public class Review
    // {
    //     public int Id { get; set; }
    //     public int RoomId { get; set; }
    //     public Room Room { get; set; }
    //     public int GuestId { get; set; } // Giả sử có User cho Guest
    //     public int Rating { get; set; } // 1-5 sao
    //     public string Comment { get; set; }
    //     public DateTime CreatedAt { get; set; }
    // }
}
