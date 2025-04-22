using System.Text;
using System;
using System.Security.Cryptography;

namespace OrderManagementApp.Services
{
public class AccountService
    {
        public virtual string HashPassword(string password)
        {
            // Chuyển đổi password thành byte[] như trước
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Sử dụng HashData thay cho ComputeHash
            byte[] hashBytes = SHA256.HashData(passwordBytes);

            return Convert.ToBase64String(hashBytes);
        }
        public virtual bool ValidateUser(string username, string password)
        {
            // Kiểm tra xác thực người dùng, có thể so sánh với cơ sở dữ liệu hoặc giả lập
            // Ví dụ đơn giản (cần thay đổi theo yêu cầu thực tế)
            if (username == "admin" && password == "password123")
            {
                return true;
            }
            return false;
        }
    }

}
