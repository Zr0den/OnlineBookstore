using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreCore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign key for Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Foreign key for Book
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
