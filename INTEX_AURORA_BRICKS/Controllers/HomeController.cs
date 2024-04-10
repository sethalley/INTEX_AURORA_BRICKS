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

        public IActionResult CrudProductAdmin()
        {
            List<Products> products = _auroraContext.Products.ToList(); // Fetch all products from the database and materialize into a list
            return View(products); // Pass the list of products to the view
        }


        // Action method to display the form for adding a new product
        public IActionResult CreateProd()
        {
            return View();
        }

        // Action method to handle the POST request for adding a new product
        [HttpPost]
        public IActionResult AddProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                // Query the database to find the highest product ID
                int highestProductId = _auroraContext.Products.Max(p => p.product_ID);

                // Increment the highest product ID by 1 to generate a new product ID
                int newProductId = highestProductId + 1;

                // Assign the generated product ID to the new record
                product.product_ID = (byte)newProductId;

                // Add the new record to the database
                _auroraContext.Products.Add(product);
                _auroraContext.SaveChanges();
                return RedirectToAction("CrudProductAdmin"); // Redirect to the product list page
            }
            return View(product);
        }

        // Action method to display the form for editing a product
        public IActionResult EditProduct(int id)
        {
            // Retrieve the product from the database based on the ID
            var product = _auroraContext.Products.FirstOrDefault(p => p.product_ID == id);

            if (product == null)
            {
                return NotFound(); // Handle case where product is not found
            }

            return View("CreateProd", product); // Pass the product to the CreateProd.cshtml view
        }

        [HttpPost]
        public IActionResult UpdateProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                _auroraContext.Products.Update(product);
                _auroraContext.SaveChanges();
                return RedirectToAction("CrudProductAdmin");
            }
            return View("CreateProd", product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(byte id)
        {
            var product = _auroraContext.Products.Find(id);
            if (product == null)
            {
                return NotFound(); // Or handle the case where the product is not found
            }

            _auroraContext.Products.Remove(product);
            _auroraContext.SaveChanges();

            return RedirectToAction("CrudProductAdmin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
