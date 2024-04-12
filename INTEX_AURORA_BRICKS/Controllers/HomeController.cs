using INTEX_AURORA_BRICKS.Infrastructure;
using INTEX_AURORA_BRICKS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Printing;

namespace INTEX_II.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuroraContext _auroraContext;
        private readonly SignInManager<Customers> _signInManager;
        private readonly UserManager<Customers> _userManager;

        public HomeController(AuroraContext auroraContext, SignInManager<Customers> signInManager, UserManager<Customers> userManager)
        {
            _auroraContext = auroraContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            IQueryable<Products> productsQuery = _auroraContext.Products;

            // Retrieve products for the specific page
            var products = productsQuery
                                .OrderBy(p => p.product_ID) // Replace Product_ID with your actual primary key or sorting preference
                                .ToList();

            var userRecommendations = _auroraContext.UserRecommendations.ToList(); // Retrieve all user recommendations

            // Define variable
            int recId;

            // Assuming currentUser is the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            // Check if currentUser is not null
            if (currentUser != null)
            {
                // Check if the user's RecId is not null
                if (currentUser.recId != null)
                {
                    recId = (int)currentUser.recId;
                }
                else
                {
                    // Query the AspNetUsers table for the user with the closest age and same gender
                    var closestUser = await _userManager.Users
                        .Where(u => u.gender == currentUser.gender) // Same gender
                        .OrderBy(u => EF.Functions.DateDiffDay(u.birth_date, currentUser.birth_date)) // Closest in age
                        .FirstOrDefaultAsync();



                    // If a closestUser is found, assign its RecId to RecId
                    if (closestUser != null)
                    {
                        recId = (int)closestUser.recId;
                    }
                    else
                    {
                        // Handle case where no closest user is found
                        // You may assign a default value or handle it based on your requirements
                        recId = 6; // For example, assigning 6 as a default value
                    }
                }
            }
            else
            {
                // Handle case where currentUser is null
                // You may assign a default value or handle it based on your requirements
                recId = 1; // For example, assigning 1 as a default value
            }

            var viewModel = new IndexViewModel
            {
                Products = products,
                UserRecommendations = userRecommendations,
                RecId = recId
            };

            return View(viewModel);
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

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var productToRemove = cart.Lines.FirstOrDefault(x => x.Products.product_ID == productId)?.Products;

            if (productToRemove != null)
            {
                cart.RemoveLine(productToRemove);
                HttpContext.Session.SetJson("cart", cart); // Save the updated cart back to session
            }

            return RedirectToAction("Cart");
        }


        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var product = _auroraContext.Products.FirstOrDefault(p => p.product_ID == productId);

            if (product != null)
            {
                var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                cart.UpdateQuantity(product, quantity); // Add a new method UpdateQuantity to Cart class
                HttpContext.Session.SetJson("cart", cart);

                // Return the updated total to the client
                return Json(cart.CalculateTotal());
            }

            return BadRequest("Product not found");
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(); // Retrieve the cart from session

            // Pass the cart directly to the view
            return View(cart);
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


        //public async Task<IActionResult> Customers()
        //{
        //    var userClaim = HttpContext.User;
        //    if (userClaim != null)
        //    {
        //        var user = await _signInManager.UserManager.GetUserAsync(userClaim);

        //        if (user != null)
        //        {
        //            return View(new Customers { UserId = user.Id });
        //        }

        //    }

        //var userClaim2 = HttpContext.User;
        //if (userClaim2 != null)
        //{
        //    var user2 = await _signInManager.UserManager.GetUserAsync(userClaim2);

        //    if (user2 != null)
        //    {
        //        var customer = _auroraContext.Customers.Where(x => x.UserId == user2.Id).FirstOrDefault();
        //        // Recommend(customer.CustomerId)
        //    }

        //    }

        //}

    



        public async Task<IActionResult> Details(byte id)
        {
            // Find the product with the specified ID
            var product = await _auroraContext.Products.FirstOrDefaultAsync(p => p.product_ID == id);

            // If the product is not found, return a not found result
            if (product == null)
            {
                return NotFound();
            }

            // Query recommendations based on the current product's ID
            var recommendations = await _auroraContext.ItemBasedRecommendations
                .Where(r => r.Product_ID == id)
                .FirstOrDefaultAsync();

            // Pass both product and recommendations to the view
            ViewBag.Recommendations = recommendations;
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
            Products newProduct = new Products(); // Create a new instance of the Products model
            return View(newProduct); // Pass the new instance to the view
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
        public IActionResult DeleteProduct(short id)
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

        public IActionResult ConfirmedOrder()
        {
            return View();
        }
        public IActionResult FlaggedOrder()
        {
            return View();
        }
    }

}