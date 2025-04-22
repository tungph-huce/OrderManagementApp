using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Controllers;
using OrderManagementApp.Models;
using OrderManagementApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagementApp.Tests
{
    public class ProductControllerTests:Controller
    {
        private ApplicationDbContext _context;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            // Tạo DbContext với InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")  // Tạo cơ sở dữ liệu trong bộ nhớ
                .Options;

            _context = new ApplicationDbContext(options);  // Khởi tạo ApplicationDbContext với InMemoryDatabase

            // Khởi tạo ProductController với DbContext đã mock
            _controller = new ProductController(_context);
        }

        [Test]
        public void Index_ShouldReturnViewResult_WithListOfProducts()
        {
            // Arrange
            var mockProductList = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _context.Products.AddRange(mockProductList);
            _context.SaveChanges();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả trả về có phải là ViewResult không
            var model = result.Model as List<Product>;
            Assert.IsNotNull(model);  // Kiểm tra xem mô hình trả về có phải là danh sách sản phẩm không
            Assert.That(model.Count,Is.EqualTo(2));  // Kiểm tra số lượng sản phẩm trong danh sách
        }

        [Test]
        public void Create_ShouldReturnViewResult_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 100 };

            // Act
            var result = _controller.Create(product) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là RedirectToAction không
            Assert.That(result.ActionName, Is.EqualTo("Index"));  // Kiểm tra xem có chuyển hướng về Index không
        }

        [Test]
        public void Create_ShouldReturnBadRequest_WhenProductIsInvalid()
        {
            // Arrange
            var product = new Product { Name = "", Price = -100 };  // Giá trị không hợp lệ

            // Act
            var result = _controller.Create(product) as ViewResult;  // Kiểm tra kết quả trả về

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là ViewResult không
            Assert.IsFalse(_controller.ModelState.IsValid);  // Kiểm tra xem ModelState có hợp lệ không
        }


        [Test]
        public void Edit_ShouldReturnViewResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là ViewResult không
            var model = result.Model as Product;
            Assert.IsNotNull(model);  // Kiểm tra xem mô hình trả về có phải là sản phẩm không
            Assert.That(model.Id,Is.EqualTo(product.Id));  // Kiểm tra xem Id có đúng không
        }

        [Test]
        public void Edit_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = _controller.Edit(999) as NotFoundResult;  // Kiểm tra sản phẩm không tồn tại

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là NotFoundResult không
        }

        [Test]
        public void Delete_ShouldReturnViewResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là ViewResult không
            var model = result.Model as Product;
            Assert.IsNotNull(model);  // Kiểm tra xem sản phẩm có được trả về không
            Assert.That(model.Id, Is.EqualTo(product.Id));  // Kiểm tra xem Id có đúng không
        }

        [Test]
        public void Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = _controller.Delete(999) as NotFoundResult;  // Kiểm tra sản phẩm không tồn tại

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là NotFoundResult không
        }

        [Test]
        public void DeleteConfirmed_ShouldRedirectToIndex_WhenProductIsFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);  // Kiểm tra xem kết quả có phải là RedirectToAction không
            Assert.That(result.ActionName, Is.EqualTo("Index"));  // Kiểm tra xem có chuyển hướng về Index không
        }

        [Test]
        public void DeleteConfirmed_ShouldNotRedirectWhenProductIsNotFound()
        {
            // Act
            var result = _controller.DeleteConfirmed(999) as RedirectToActionResult;

            // Assert
            Assert.IsNull(result);  // Kiểm tra xem nếu không tìm thấy sản phẩm thì không chuyển hướng
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
