using INTEX_AURORA_BRICKS.Infrastructure;
using INTEX_AURORA_BRICKS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace INTEX_II.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuroraContext _auroraContext;

        public HomeController(AuroraContext auroraContext)
        {
            _auroraContext = auroraContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        //public IActionResult Cart()
        //{
        //    var cart = _auroraContext.Cart;

        //    // Assuming _auroraContext.Cart is the Cart model you want to pass to the view
        //    return View(cart);
        //}

        //GTP STUFF
        //public IActionResult Cart()
        //{
        //    // Assuming _auroraContext.Cart represents the cart data
        //    var cart = _auroraContext.Cart.FirstOrDefault(); // You may need to adjust this based on your data structure

        //    return View(cart);
        //}

        //GEMINI
        public IActionResult Cart()
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(); // Create an empty cart
            return View(cart);
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _auroraContext.Products.FirstOrDefault(p => p.product_ID == productId);

            if (product != null)
            {
                var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(); // You should implement a method to retrieve the cart (e.g., from session or database)
                cart.AddItem(product, 1); // Add the product to the cart with a quantity of 1
                HttpContext.Session.SetJson("cart", cart); // You should implement a method to save the cart (e.g., to session or database)
            }

            return RedirectToAction("Cart"); // Redirect to the Cart action
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products(int page = 1, int pageSize = 10)
        {
            // Calculate the total number of products
            var totalProducts = _auroraContext.Products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Ensure the page number is within the valid range
            page = Math.Max(page, 1); // Ensure page is at least 1
            page = Math.Min(page, totalPages); // Ensure page does not exceed totalPages

            // Retrieve the products for the specific page
            var products = _auroraContext.Products
                                .OrderBy(p => p.product_ID)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var viewModel = new ProductsViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        [HttpPost] // Handle POST requests for changing items per page
        public IActionResult Products(int pageSize)
        {
            // Redirect back to the Products action with the new pageSize
            return RedirectToAction("Products", new { pageSize });
        }


        public IActionResult Details(byte id)
        {
            var product = _auroraContext.Products.FirstOrDefault(p => p.product_ID == id);
            if (product == null)
            {
                return NotFound(); // Handle the case where the product with the specified ID is not found
            }
            return View(product);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
