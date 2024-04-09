using INTEX_AURORA_BRICKS.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products(string category = null, string color = null, string price = null, int page = 1, int pageSize = 10)
        {
            // Fetch distinct categories and colors from your database
            ViewBag.Categories = _auroraContext.Products.Select(p => p.category).Distinct().ToList();
            ViewBag.Colors = _auroraContext.Products.Select(p => p.primary_color).Distinct().ToList();

            IQueryable<Products> productsQuery = _auroraContext.Products;

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.category == category);
            }

            // Filter by color
            if (!string.IsNullOrEmpty(color))
            {
                productsQuery = productsQuery.Where(p => p.primary_color == color);
            }

            // Filter by price
            if (!string.IsNullOrEmpty(price))
            {
                if (price == "min")
                {
                    productsQuery = productsQuery.Where(p => p.price >= 0 && p.price <= 50);
                }
                else if (price == "standard")
                {
                    productsQuery = productsQuery.Where(p => p.price > 50 && p.price <= 100);
                }
                else if (price == "high")
                {
                    productsQuery = productsQuery.Where(p => p.price > 100 && p.price <= 200);
                }
                else if (price == "reallyHigh")
                {
                    productsQuery = productsQuery.Where(p => p.price > 200 && p.price <= 300);
                }
                else if (price == "megaHigh")
                {
                    productsQuery = productsQuery.Where(p => p.price > 300);
                }
                // Add more conditions as needed
            }

            // Calculate total number of pages
            var totalProducts = productsQuery.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Ensure the page number is within the valid range
            page = Math.Max(page, 1);
            page = Math.Min(page, totalPages);

            // Retrieve products for the specific page
            var products = productsQuery
                                .OrderBy(p => p.product_ID) // Replace Product_ID with your actual primary key or sorting preference
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
