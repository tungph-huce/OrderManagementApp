using System.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
using OrderManagementApp.Models;

namespace OrderManagementApp.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {

                if (!context.Users.Any(u=>u.Username=="admin"))
                {
                    context.Users.Add(new User
                    {
                        Username = "admin",
                        PasswordHash = HashPassword("123456"),
                        Role = "Admin"
                    });

                    context.SaveChanges();
                }
              context.SaveChanges();
      //      System.Diagnostics.Debug.WriteLine("User admin đã được seed.");
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
