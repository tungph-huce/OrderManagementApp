📦 Order Management System (.NET)
📌 Giới thiệu

Order Management System là một ứng dụng quản lý đơn hàng được xây dựng bằng ASP.NET Core, áp dụng các nguyên lý OOP, Layered Architecture và các Design Pattern phổ biến như:

Repository Pattern
Unit of Work
Dependency Injection
Dự án mô phỏng một hệ thống quản lý đơn hàng cơ bản, phục vụ mục đích học tập, giảng dạy và nghiên cứu kiến trúc phần mềm.
🎯 Mục tiêu dự án
Hiểu và áp dụng Class Diagram → Code
Phân tách rõ Domain – Application – Infrastructure – Presentation
Thực hành xây dựng API / MVC theo chuẩn doanh nghiệp
Kết nối và thao tác dữ liệu với SQL Server + Entity Framework Core
🏗️ Kiến trúc hệ thống
Dự án sử dụng kiến trúc Layered Architecture (Monolithic):
```text
Presentation Layer
│
├── Controllers
│
Application Layer
│
├── Services
│
Domain Layer
│
├── Entities
│
Infrastructure Layer
│
├── Repositories
├── DbContext
```
🔹 Domain Model

User

Order

OrderItem

Product

🔹 Application Layer

OrderService

UserService

🔹 Infrastructure Layer

OrderRepository

GenericRepository

AppDbContext

🔹 Presentation Layer

OrderController

UserController

📐 Class Diagram (Mermaid)
classDiagram
    class User {
        +int Id
        +string Name
    }

    class Order {
        +int Id
        +DateTime OrderDate
        +decimal TotalAmount
    }

    class OrderItem {
        +int Quantity
        +decimal Price
    }

    class Product {
        +int Id
        +string Name
        +decimal Price
    }

    User "1" --> "0..*" Order
    Order "1" *-- "1..*" OrderItem
    OrderItem --> Product

    class OrderController
    class OrderService
    class OrderRepository

    OrderController ..> OrderService
    OrderService ..> OrderRepository

🛠️ Công nghệ sử dụng
Công nghệ	Mô tả
.NET	.NET 6 / 7 / 8
ASP.NET Core	Web API / MVC
Entity Framework Core	ORM
SQL Server	Cơ sở dữ liệu
Swagger	Test API
Mermaid	Vẽ sơ đồ
⚙️ Cài đặt & Chạy dự án
1️⃣ Clone repository
git clone https://github.com/your-username/order-management.git
cd order-management

2️⃣ Cấu hình database

Cập nhật appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=OrderManagementDb;Trusted_Connection=True;"
}

3️⃣ Migration & Update DB
dotnet ef migrations add InitialCreate
dotnet ef database update

4️⃣ Chạy ứng dụng
dotnet run

➡️ Truy cập Swagger:
https://localhost:5001/swagger

📂 Cấu trúc thư mục
```text
OrderManagement
│
├── Controllers
│   └── OrderController.cs
│
├── Services
│   └── OrderService.cs
│
├── Repositories
│   └── OrderRepository.cs
│
├── Models
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Product.cs
│
├── Data
│   └── AppDbContext.cs
│
└── Program.cs
```
🧪 Chức năng chính

CRUD User

CRUD Product

Tạo đơn hàng nhiều sản phẩm

Tính tổng tiền đơn hàng

Xem danh sách & chi tiết đơn hàng

📚 Kiến thức áp dụng

UML Class Diagram

SOLID Principles

Dependency Injection

Repository Pattern

Unit of Work

RESTful API Design

👨‍🏫 Phục vụ học tập

Dự án phù hợp cho:

Sinh viên CNTT

Môn Kiến trúc & Thiết kế phần mềm

Thực hành UML → Code

Đồ án môn học / đồ án tốt nghiệp

📄 License

This project is for educational purposes only.
