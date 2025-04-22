using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApp.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using OrderManagementApp.Services;
using OrderManagementApp.Models;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AccountService _userService;
    public AccountController(ApplicationDbContext context, AccountService userService)
    {
        _context = context;
        _userService = userService;        
    }

    public IActionResult Login() => View();
    [HttpPost]

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        // Kiểm tra xem loginModel có null hoặc các trường trống không
        if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
        {
            return BadRequest();  // Trả về BadRequest nếu dữ liệu không hợp lệ
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();  // Trả về BadRequest nếu ModelState không hợp lệ
        }

        var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Username);
        if (user == null) return View();  // Trả về View khi không tìm thấy người dùng

        string hashed = _userService.HashPassword(loginModel.Password);
        if (user.PasswordHash != hashed)
        {
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
            return View();  // Trả về View khi mật khẩu không đúng
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Product");
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
    public IActionResult AccessDenied()
    {
        // Ghi log người dùng bị từ chối
        string username = User.Identity.IsAuthenticated ? User.Identity.Name : "Người dùng chưa đăng nhập";
        string role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Không rõ";
        string url = HttpContext.Request.Headers["Referer"].ToString();

        Console.WriteLine($"⚠️ AccessDenied - User: {username}, Role: {role}, Từ URL: {url}, Thời điểm: {DateTime.Now}");

        return View();
    }


    //private string HashPassword(string password)
    //{
    //    using var sha = SHA256.Create();
    //    var bytes = Encoding.UTF8.GetBytes(password);
    //    var hash = sha.ComputeHash(bytes);
    //    return Convert.ToBase64String(hash);
    //}
}
