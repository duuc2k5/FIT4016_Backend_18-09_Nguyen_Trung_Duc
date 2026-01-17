using OrderManagementApp.Models;
using OrderManagementApp.Models.Entities;

namespace OrderManagementApp.Data
{
    /// <summary>
    /// SeedData class for initializing database with sample data
    /// Creates 15+ products and 40+ orders for testing CRUD operations
    /// </summary>
    public class SeedData
    {
        /// <summary>
        /// Initialize database with sample products and orders
        /// Only runs if database is empty (no data already exists)
        /// </summary>
        /// <param name="context">Database context</param>
        public static void Initialize(OrderManagementContext context)
        {
            // Prevent duplicate seeding
            if (context.Products.Any() || context.Orders.Any())
            {
                return;
            }

            // Create sample products with realistic data
            var products = new List<Product>
            {
                new Product { ProductName = "Laptop Dell XPS 13", Description = "Ultra-portable laptop", Price = 999.99m, StockQuantity = 50 },
                new Product { ProductName = "Mouse Logitech MX", Description = "Wireless mouse", Price = 99.99m, StockQuantity = 200 },
                new Product { ProductName = "Keyboard Mechanical RGB", Description = "Gaming keyboard", Price = 149.99m, StockQuantity = 150 },
                new Product { ProductName = "Monitor LG 4K 27\"", Description = "4K display monitor", Price = 499.99m, StockQuantity = 75 },
                new Product { ProductName = "USB-C Hub", Description = "Multi-port USB hub", Price = 49.99m, StockQuantity = 300 },
                new Product { ProductName = "Webcam Logitech 4K", Description = "Ultra HD webcam", Price = 199.99m, StockQuantity = 100 },
                new Product { ProductName = "Headphones Sony WH1000XM5", Description = "Noise-cancelling headphones", Price = 399.99m, StockQuantity = 80 },
                new Product { ProductName = "SSD Samsung 1TB", Description = "1TB NVMe SSD", Price = 129.99m, StockQuantity = 250 },
                new Product { ProductName = "Desk Lamp LED", Description = "Smart desk lamp", Price = 79.99m, StockQuantity = 180 },
                new Product { ProductName = "External HDD 4TB", Description = "4TB backup storage", Price = 89.99m, StockQuantity = 120 },
                new Product { ProductName = "Graphics Card RTX 4060", Description = "Gaming GPU", Price = 299.99m, StockQuantity = 40 },
                new Product { ProductName = "Power Supply 850W", Description = "Modular PSU", Price = 139.99m, StockQuantity = 90 },
                new Product { ProductName = "Motherboard MSI B850", Description = "AMD AM5 motherboard", Price = 249.99m, StockQuantity = 60 },
                new Product { ProductName = "RAM DDR5 32GB", Description = "32GB memory kit", Price = 199.99m, StockQuantity = 110 },
                new Product { ProductName = "Cooling Fan Arctic Liquid", Description = "All-in-one CPU cooler", Price = 159.99m, StockQuantity = 70 }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // Create orders with realistic data
            var orders = new List<Order>();
            var random = new Random();
            var baseDate = new DateTime(2026, 1, 1);

            // Create sample orders (40+) with realistic data
            int orderCounter = 1;
            for (int i = 0; i < 40; i++)
            {
                var productId = products[random.Next(products.Count)].ProductId;
                var product = products.First(p => p.ProductId == productId);
                
                // Random quantity between 1 and 10, but not exceeding stock
                var quantity = random.Next(1, Math.Min(product.StockQuantity / 5 + 1, 10));
                
                // Random order date within January 2026
                var orderDate = baseDate.AddDays(random.Next(0, 17));
                
                // 70% chance of having delivery date, 30% for pending orders
                var deliveryDate = random.Next(0, 100) > 30 
                    ? (DateTime?)orderDate.AddDays(random.Next(1, 8)) 
                    : null;

                var order = new Order
                {
                    OrderNumber = $"ORD-{orderDate:yyyyMMdd}-{orderCounter:D4}",
                    CustomerName = GetRandomCustomerName(random),
                    CustomerEmail = GetRandomEmail(random, i),
                    ProductId = productId,
                    Quantity = quantity,
                    OrderDate = orderDate,
                    DeliveryDate = deliveryDate,
                    CreatedDate = orderDate
                };

                orders.Add(order);
                orderCounter++;
                
                // Reset counter to 4 digits format
                if (orderCounter > 9999) 
                    orderCounter = 1;
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        /// <summary>
        /// Generate random customer name from predefined lists
        /// </summary>
        /// <param name="random">Random instance</param>
        /// <returns>Random customer name</returns>
        private static string GetRandomCustomerName(Random random)
        {
            // Predefined names for generating realistic customer names
            var firstNames = new[] 
            { 
                "John", "Jane", "Michael", "Sarah", "David", "Emily", 
                "Robert", "Lisa", "James", "Mary", "William", "Patricia", 
                "Richard", "Jennifer", "Thomas", "Linda" 
            };
            var lastNames = new[] 
            { 
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", 
                "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", 
                "Gonzalez", "Wilson", "Anderson", "Thomas" 
            };

            return $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
        }

        /// <summary>
/// <summary>
        /// Generate random email address from predefined formats
        /// </summary>
        /// <param name="random">Random instance</param>
        /// <param name="index">Order index for unique email suffix</param>
        /// <returns>Random email address</returns>
        private static string GetRandomEmail(Random random, int index)
        {
            // Predefined domains and base names for generating realistic emails
            var domains = new[] { "gmail.com", "yahoo.com", "outlook.com", "company.com", "email.net" };
            var baseNames = new[] { "customer", "user", "client", "buyer", "shopper", "order" };

            return $"{baseNames[random.Next(baseNames.Length)]}{index + 1000}@{domains[random.Next(domains.Length)]}";
        }
    }
}
