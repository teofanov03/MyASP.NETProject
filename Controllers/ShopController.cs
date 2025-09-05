using Microsoft.AspNetCore.Mvc;
using TodoBlog.Models;
using System.Text.Json;

namespace TodoBlog.Controllers
{
    public class ShopController : Controller
    {
        private readonly List<Product> _products = new()
        {
            new Product {
                 Id = 1,
                 Name = "Eco-friendly Notebook",
                Description = "A stylish notebook made from recycled materials, perfect for journaling or work notes.",
                Price = 10.5m,
                ImageUrl = "/Images/Eco-friendly-Notebook.jpg"
            },
            new Product {
                Id = 2,
                Name = "Wireless Earbuds",
                Description = "High-quality Bluetooth earbuds with clear sound, long battery life, and a sleek design.",
                Price = 25.0m,
                ImageUrl = "/Images/Wireless-Earbuds.jpg"
            },
            new Product {
                Id = 3,
                Name = "Ceramic Coffee Mug",
                Description = "A durable ceramic mug with a comfortable handle, ideal for hot drinks at home or office.",
                Price = 15.75m,
                ImageUrl = "/Images/Ceramic-Coffee-Mug.jpg"
            },
            new Product {
                Id = 4,
                Name = "Stainless Steel Water Bottle",
                Description = "Reusable water bottle keeping your drinks cold for 24h or hot for 12h, perfect for daily use.",
                Price = 18.0m,
                ImageUrl = "/Images/Stainless-Steel-Water-Bottle.jpg"
            },
            new Product {
                Id = 5,
                Name = "Portable Phone Charger",
                Description = "Compact power bank with fast-charging capability, ideal for travel or emergencies.",
                Price = 30.0m,
                ImageUrl = "/Images/Portable-Phone-Charger.jpg"
            },
            new Product {
                Id = 6,
                Name = "LED Desk Lamp",
                Description = "Adjustable LED lamp with multiple brightness levels, designed for reading and working.",
                Price = 22.5m,
                ImageUrl = "/Images/Led-Desk-Lamp.jpg"
            },
            new Product {
                Id = 7,
                Name = "Canvas Tote Bag",
                Description = "Durable canvas bag with stylish print, perfect for shopping or carrying books.",
                Price = 12.0m,
                ImageUrl = "/Images/Canvas-Tote-Bag.jpg"
            }
        };

        private const string CartSessionKey = "Cart";

        public IActionResult Index()
        {
            var cart = GetCart();
            SaveCart(cart);
            return View(_products);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, cartCount = cart.Sum(c => c.Quantity) });
            }

            
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(c => c.Total);
            return View(cart);
        }

        [HttpPost]
        
        public IActionResult IncreaseQuantity(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            SaveCart(cart);
            return Json(new { success = true, quantity = item?.Quantity, total = item?.Total, cartTotal = cart.Sum(c => c.Total) });
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                item.Quantity--;
                
            }
            SaveCart(cart);
            return Json(new { success = true, quantity = item?.Quantity, total = item?.Total, cartTotal = cart.Sum(c => c.Total) });
        }

        
        [HttpPost]
        public IActionResult RemoveItem(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item); 
            }
            SaveCart(cart);
            return Json(new { success = true, cartTotal = cart.Sum(c => c.Total) });
        
       

        }
        [HttpGet]
        public IActionResult CartCount()
        {
            var cart = GetCart();
            return Json(cart.Sum(c => c.Quantity));
        }
        private List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString(CartSessionKey);
            return session == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(session)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }
    }
}

