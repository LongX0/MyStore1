using Microsoft.AspNetCore.Mvc;
using MyStore.Data;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartKey) ?? new List<CartItem>();

            
            foreach (var item in cart)
            {
                item.Product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
            }

            return View(cart);
        }

        
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartKey) ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson(CartKey, cart);
            return RedirectToAction("Index");
        }

        
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var itemToRemove = cart.FirstOrDefault(i => i.ProductId == id);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson(CartKey, cart);
            }

            return RedirectToAction("Index");
        }

        
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartKey);
            return RedirectToAction("Index");
        }
    }
}
