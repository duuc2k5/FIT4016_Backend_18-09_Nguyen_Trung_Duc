using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementApp.Models.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Order number is required")]
        [StringLength(20, MinimumLength = 17, ErrorMessage = "Order number must be in format ORD-YYYYMMDD-XXXX")]
        [RegularExpression(@"^ORD-\d{8}-\d{4}$", ErrorMessage = "Order number must be in format ORD-YYYYMMDD-XXXX (e.g., ORD-20260117-0001)")]
        public string OrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2 and 100 characters")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product is required")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual Product? Product { get; set; }

        // Property for status display (computed)
        [NotMapped]
        public string Status
        {
            get => DeliveryDate.HasValue ? "Delivered" : "Pending";
        }
    }
}
