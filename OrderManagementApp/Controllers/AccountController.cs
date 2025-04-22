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

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null) return View();

        string hashed = HashPassword(password);
        if (user.PasswordHash != hashed)
        {
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role) // 👈 Thêm dòng này
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


    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
