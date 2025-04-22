using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApp.Data;
using OrderManagementApp.Models;
using OrderManagementApp.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OrderManagementApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AccountService _userService;
        public UserController(ApplicationDbContext context, AccountService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index() => View(_context.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest(); // Trả về BadRequest nếu mật khẩu là null hoặc rỗng
            }

            // Cung cấp giá trị cho Role nếu không có
            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "User";  // Gán giá trị mặc định cho Role
            }

            if (ModelState.IsValid)
            {
                user.PasswordHash = _userService.HashPassword(user.PasswordHash);
                _context.Users.Add(user);
                _context.SaveChanges();
                // Trả về "CreateUser" view nếu ModelState hợp lệ
                return View("CreateUser", user);  // Đảm bảo trả về đúng ViewName
            }

            // Trả về View với "CreateUser" nếu ModelState không hợp lệ
            return View("CreateUser", user);  // Đảm bảo trả về đúng ViewName
        }


        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        //private string HashPassword(string password)
        //{
        //    using var sha = System.Security.Cryptography.SHA256.Create();
        //    var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        //    var hash = sha.ComputeHash(bytes);
        //    return Convert.ToBase64String(hash);
        //}

    }
}
