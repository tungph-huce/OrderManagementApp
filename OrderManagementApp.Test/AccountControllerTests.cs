using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApp.Controllers;
using OrderManagementApp.Services;
using OrderManagementApp.Models;
using OrderManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementApp.Tests
{
    public class AccountControllerTests
    {
        private Mock<AccountService> _mockUserService;
        //private Mock<ApplicationDbContext> _mockDbContext; // Mock ApplicationDbContext
        private ApplicationDbContext _context;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            // Mô phỏng InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Tạo một cơ sở dữ liệu trong bộ nhớ
                .Options;

            _context = new ApplicationDbContext(options);

            // Mô phỏng AccountService
            _mockUserService = new Mock<AccountService>();

            // Khởi tạo AccountController với các phụ thuộc đã mock
            _controller = new AccountController(_context, _mockUserService.Object);
        }

        [Test]
        public async Task Login_ShouldReturnViewResult_WhenModelIsInvalid()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "", Password = "" };  // Mô hình không hợp lệ

            // Act
            var result = await _controller.Login(loginModel) as ViewResult;  // Gọi phương thức Login và đợi kết quả trả về

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra kết quả trả về không phải null
            Assert.That(result.ViewName, Is.EqualTo("Login"));  // Kiểm tra nếu ViewName là "Login"
        }


        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "", Password = "" };  // Mô hình không hợp lệ

            // Act
            var result = await _controller.Login(loginModel);  // Gọi phương thức Login và đợi kết quả trả về

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);  // Kiểm tra xem có trả về BadRequestResult khi mô hình không hợp lệ
        }


        [TearDown]
        public void TearDown()
        {
            // Giải phóng các tài nguyên (nếu cần)
            _controller?.Dispose();
            _context?.Dispose();
        }

    }
}
