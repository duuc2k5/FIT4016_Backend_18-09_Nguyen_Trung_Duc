using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Models;
using OrderManagementApp.Models.Entities;

namespace OrderManagementApp.Controllers
{
    /// <summary>
    /// OrdersController handles all CRUD operations for orders
    /// Implements Create, Read, Update, Delete functionality with validation
    /// </summary>
    public class OrdersController : Controller
    {
        private readonly OrderManagementContext _context;
        private const int PageSize = 10;

        public OrdersController(OrderManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Orders - Display paginated list of all orders with search capability
        /// </summary>
        /// <param name="searchString">Optional search term for Order Number or Customer Name</param>
        /// <param name="pageNumber">Current page number for pagination (default: 1)</param>
        /// <returns>View with orders list and pagination info</returns>
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var orders = _context.Orders.Include(o => o.Product).AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => 
                    o.OrderNumber.Contains(searchString) || 
                    o.CustomerName.Contains(searchString));
            }

            var totalCount = await orders.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            // Validate and adjust page number
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages) pageNumber = totalPages > 0 ? totalPages : 1;

            var ordersForPage = await orders
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewData["SearchString"] = searchString;
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalCount"] = totalCount;

            return View(ordersForPage);
        }

        /// <summary>
        /// GET: Orders/Create - Display form to create new order
        /// </summary>
        /// <returns>Create view with available products</returns>
        public IActionResult Create()
        {
            ViewData["Products"] = _context.Products.ToList();
            return View();
        }

        /// <summary>
        /// POST: Orders/Create - Save new order to database with validation
        /// </summary>
        /// <param name="order">Order data to create</param>
        /// <returns>Redirect to Index on success, or Create view with errors on failure</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNumber,CustomerName,CustomerEmail,ProductId,Quantity,OrderDate,DeliveryDate")] Order order)
        {
            // Validate order data
            var validationErrors = ValidateOrder(order, isEdit: false);
            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Order created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle database constraint violations
                    if (ex.InnerException?.Message.Contains("Unique") == true)
                    {
                        ModelState.AddModelError("OrderNumber", "This order number already exists.");
                    }
                    else if (ex.InnerException?.Message.Contains("Email") == true)
                    {
                        ModelState.AddModelError("CustomerEmail", "This email is already used in another order.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the order.");
                    }
                }
            }

            ViewData["Products"] = _context.Products.ToList();
            return View(order);
        }

        /// <summary>
        /// GET: Orders/Edit/5 - Display form to edit existing order
        /// </summary>
        /// <param name="id">Order ID to edit</param>
        /// <returns>Edit view with order data and available products</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["Products"] = _context.Products.ToList();
            return View(order);
        }

        /// <summary>
        /// POST: Orders/Edit/5 - Update order with validation
        /// Cannot modify: Order Number, Product, Order Date (key fields)
        /// </summary>
        /// <param name="id">Order ID being edited</param>
        /// <param name="order">Updated order data</param>
        /// <returns>Redirect to Index on success, or Edit view with errors on failure</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderNumber,CustomerName,CustomerEmail,ProductId,Quantity,OrderDate,DeliveryDate")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            // Retrieve original order to preserve non-editable fields
            var originalOrder = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
            if (originalOrder == null)
            {
                return NotFound();
            }

            // Preserve original values for non-editable fields
            order.OrderNumber = originalOrder.OrderNumber;
            order.ProductId = originalOrder.ProductId;
            order.OrderDate = originalOrder.OrderDate;

            // Validate updated order data
            var validationErrors = ValidateOrder(order, isEdit: true);
            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Order updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("Email") == true)
                    {
                        ModelState.AddModelError("CustomerEmail", "This email is already used in another order.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while updating the order.");
                    }
                }
            }

            ViewData["Products"] = _context.Products.ToList();
            return View(order);
        }

        /// <summary>
        /// GET: Orders/Delete/5 - Display confirmation dialog before deleting order
        /// </summary>
        /// <param name="id">Order ID to delete</param>
        /// <returns>Delete confirmation view with order details</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// POST: Orders/Delete/5 - Delete order after confirmation
        /// </summary>
        /// <param name="id">Order ID to delete</param>
        /// <returns>Redirect to Index with success or error message</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Order deleted successfully.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while deleting the order: {ex.Message}";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check if order exists in database
        /// </summary>
        /// <param name="id">Order ID to check</param>
        /// <returns>True if order exists, false otherwise</returns>
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        /// <summary>
        /// Validate order data according to business rules
        /// </summary>
        /// <param name="order">Order to validate</param>
        /// <param name="isEdit">True if validating for update, false for create</param>
        /// <returns>Dictionary of validation errors (empty if valid)</returns>
        private Dictionary<string, string> ValidateOrder(Order order, bool isEdit = false)
        {
            var errors = new Dictionary<string, string>();

            // Validate Order Number format: ORD-YYYYMMDD-XXXX
            if (string.IsNullOrEmpty(order.OrderNumber))
            {
                errors["OrderNumber"] = "Order number is required.";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(order.OrderNumber, @"^ORD-\d{8}-\d{4}$"))
            {
                errors["OrderNumber"] = "Order number must be in format ORD-YYYYMMDD-XXXX (e.g., ORD-20260117-0001).";
            }
            else if (!isEdit && _context.Orders.Any(o => o.OrderNumber == order.OrderNumber))
            {
                errors["OrderNumber"] = "This order number already exists.";
            }

            // Validate Customer Name
            if (string.IsNullOrEmpty(order.CustomerName))
            {
                errors["CustomerName"] = "Customer name is required.";
            }
            else if (order.CustomerName.Length < 2 || order.CustomerName.Length > 100)
            {
                errors["CustomerName"] = "Customer name must be between 2 and 100 characters.";
            }

            // Validate Customer Email
            if (string.IsNullOrEmpty(order.CustomerEmail))
            {
                errors["CustomerEmail"] = "Customer email is required.";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(order.CustomerEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errors["CustomerEmail"] = "Please enter a valid email address.";
            }
            else
            {
                var emailExists = isEdit 
                    ? _context.Orders.Any(o => o.CustomerEmail == order.CustomerEmail && o.OrderId != order.OrderId)
                    : _context.Orders.Any(o => o.CustomerEmail == order.CustomerEmail);
                
                if (emailExists)
                {
                    errors["CustomerEmail"] = "This email is already used in another order.";
                }
            }

            // Validate Product exists
            if (order.ProductId <= 0)
            {
                errors["ProductId"] = "Product is required.";
            }
            else if (!_context.Products.Any(p => p.ProductId == order.ProductId))
            {
                errors["ProductId"] = "Selected product does not exist.";
            }

            // Validate Quantity
            if (order.Quantity <= 0)
            {
                errors["Quantity"] = "Quantity must be greater than 0.";
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == order.ProductId);
                if (product != null && order.Quantity > product.StockQuantity)
                {
                    errors["Quantity"] = $"Quantity cannot exceed available stock ({product.StockQuantity} units).";
                }
            }

            // Validate Order Date
            if (order.OrderDate == default)
            {
                errors["OrderDate"] = "Order date is required.";
            }
            else if (order.OrderDate.Date > DateTime.Now.Date)
            {
                errors["OrderDate"] = "Order date cannot be greater than today.";
            }

            // Validate Delivery Date
            if (order.DeliveryDate.HasValue && order.DeliveryDate.Value.Date < order.OrderDate.Date)
            {
                errors["DeliveryDate"] = "Delivery date must be greater than or equal to order date.";
            }

            return errors;
        }
    }
}
