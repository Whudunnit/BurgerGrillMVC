using Burger_Grill.Models;
using BurgerGrill.Data;
using BurgerGrill.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Burger_Grill.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Repository<Product> _products;
        private Repository<Order> _orders;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _products = new Repository<Product>(context);
            _orders = new Repository<Order>(context);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            //Retrieve or create an OrderViewModel from session or other state management
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };


            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var product = await _context.Products.FindAsync(prodId);
            if (product == null)
            {
                return NotFound();
            }

            // Retrieve or create an OrderViewModel from session or other state management
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllAsync()
            };

            // Check if the product is already in the order
            var existingItem = model.OrderItems.FirstOrDefault(oi => oi.ProductId == prodId);

            // If the product is already in the order, update the quantity
            if (existingItem != null)
            {
                existingItem.Quantity += prodQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Quantity = prodQty,
                    ProductName = product.Name
                });
            }

            // Update the total amount
            model.TotalAmount = model.OrderItems.Sum(oi => oi.Price * oi.Quantity);

            // Save updated OrderViewModel to session
            HttpContext.Session.Set("OrderViewModel", model);

            // Redirect back to Create to show updated order items
            return RedirectToAction("Create", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cart()
        {

            // Retrieve the OrderViewModel from session or other state management
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");

            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            // Retrieve the OrderViewModel from session
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }

            // Create a new Order entity
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManager.GetUserId(User),
                OrderItems = new List<OrderItem>() // Initialize the OrderItems list
            };

            foreach (var item in model.OrderItems)
            {
                // Find the product in the database
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                {
                    // If product is not found, return an error
                    return NotFound($"Product with ID {item.ProductId} not found.");
                }

                // Check if there is enough stock for the order
                if (product.Stock < item.Quantity)
                {
                    // Store the error message in TempData
                    TempData["ErrorMessage"] = $"Not enough stock for product {product.Name}. Only {product.Stock} left.";

                    // Redirect the user back to the Cart view
                    return RedirectToAction("Cart");
                }
                // Deduct the ordered quantity from the product stock
                product.Stock -= item.Quantity;

                // Add the item to the OrderItems list
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            // Save the updated product stock and the new order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the OrderViewModel from session
            HttpContext.Session.Remove("OrderViewModel");

            // Redirect to the Order Confirmation page
            return RedirectToAction("ViewOrders");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager.GetUserId(User);

            var userOrders = await _orders.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Product"
            });

            return View(userOrders);
        }

        [HttpPost]
        [Authorize]
        public IActionResult ClearCart()
        {
            // Remove the "OrderViewModel" from session to clear the cart
            HttpContext.Session.Remove("OrderViewModel");

            return RedirectToAction("Create");
        }



    }
}
