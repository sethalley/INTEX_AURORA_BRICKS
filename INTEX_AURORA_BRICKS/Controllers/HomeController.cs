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

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = _auroraContext.Products.ToList();
            return View(products);
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
