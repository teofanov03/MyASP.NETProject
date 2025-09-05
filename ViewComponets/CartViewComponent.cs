using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TodoBlog.Models;

namespace TodoBlog.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private const string CartSessionKey = "Cart";

        public IViewComponentResult Invoke()
        {
            var session = HttpContext.Session.GetString(CartSessionKey);
            var cart = session == null
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(session)!;


            int count = cart.Sum(c => c.Quantity);

            return View(count);
        }
    }
}
