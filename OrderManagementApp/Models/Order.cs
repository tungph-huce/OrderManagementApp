using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        [NotMapped]
        public decimal TotalAmount => OrderDetails != null ? 
            System.Linq.Enumerable.Sum(OrderDetails, d => d.Quantity * d.UnitPrice) : 0;
    }

    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
