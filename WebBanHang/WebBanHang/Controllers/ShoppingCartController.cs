using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class ShoppingCartController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public ShoppingCartController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
        return View(cart);
    }

    public IActionResult AddToCart(int productId, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return NotFound();
        }

        var cartItem = new CartItem
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = quantity
        };

        var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
        cart.AddItem(cartItem);
        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Index");
    }

    public IActionResult Checkout()
    {
        return View(new Order());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(Order order)
    {
        var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("Index");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Index");
        }

        order.UserId = user.Id;
        order.OrderDate = DateTime.UtcNow;
        order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
        order.OrderDetails = cart.Items.Select(i => new OrderDetail
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        HttpContext.Session.Remove("Cart");

        return View("OrderCompleted", order.Id);
    }

}
