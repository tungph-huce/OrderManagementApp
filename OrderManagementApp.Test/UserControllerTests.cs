using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApp.Controllers;
using OrderManagementApp.Services;
using OrderManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OrderManagementApp.Data;

namespace OrderManagementApp.Tests
{
    public class UserControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<AccountService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            // Tạo DbContext với InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")  // Tạo cơ sở dữ liệu trong bộ nhớ
                .Options;

            _context = new ApplicationDbContext(options);  // Khởi tạo ApplicationDbContext với InMemoryDatabase

            // Mô phỏng AccountService
            _mockUserService = new Mock<AccountService>();

            // Khởi tạo UserController với các phụ thuộc đã mock
            _controller = new UserController(_context, _mockUserService.Object);
        }

        [Test]
        public void CreateUser_ShouldReturnViewResult_WhenPasswordIsValid()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                PasswordHash = "ValidPassword123"  // Đây là giá trị mà HashPassword sẽ nhận
            };

            var hashedPassword = "hashed_password_mock";

            // Đảm bảo rằng HashPassword sẽ được gọi với giá trị "ValidPassword123"
            _mockUserService.Setup(s => s.HashPassword("ValidPassword123")).Returns(hashedPassword);

            // Act
            var result = _controller.Create(user) as ViewResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem result có phải là ViewResult không
            Assert.That(result.ViewName, Is.EqualTo("CreateUser"));  // Kiểm tra nếu ViewName là "CreateUser"

            // Kiểm tra xem HashPassword có được gọi với giá trị "ValidPassword123"
            _mockUserService.Verify(s => s.HashPassword("ValidPassword123"), Times.Once);

            // Đảm bảo HashPassword đã được gọi với giá trị đúng
            _mockUserService.Verify(s => s.HashPassword(It.IsAny<string>()), Times.Once);
        }




        [Test]
        public void CreateUser_ShouldReturnBadRequest_WhenPasswordIsNull()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                PasswordHash = null // Đặt mật khẩu là null
            };

            // Act
            var result = _controller.Create(user); // Gọi phương thức Create với đối tượng User

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result); // Kiểm tra xem có trả về BadRequest không
        }
        [TearDown]
        public void TearDown()
        {
            // Giải phóng các tài nguyên (nếu cần)
            _controller?.Dispose();
            _context.Dispose();
        }
    }
}
