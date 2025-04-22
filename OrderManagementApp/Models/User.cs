using System.ComponentModel.DataAnnotations;

namespace OrderManagementApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; } // Lưu mật khẩu đã mã hóa
        [Required]
        public string Role { get; set; } // Thêm dòng này (Admin/User)
    }
}
