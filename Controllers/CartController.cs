using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCartTest.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShoppingCartTest.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Cart> carts = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/AddToCart/GetALl");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cart>>(readTask.Result);
                    readTask.Wait();
                    carts = deserialized;
                }
                else
                {
                    carts = Enumerable.Empty<Cart>();
                    ModelState.AddModelError(string.Empty, "Cart not found.");
                }
            }
            return View(carts);
        }
     
        public IActionResult Create() {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create(Product prod)
        {
            var product = new ProductModel();
            product.Name = prod.Name;
            product.Description = prod.Description;
            product.Description = prod.Description;
            product.Catagory = prod.Catagory;
            product.Price = prod.Price;
            product.Quantity = prod.Quantity;
            using (var client = new HttpClient())
            {


                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                using (var responce = await client.PostAsJsonAsync($"api/AddToCart/AddCart", product)) 
                {

                    return RedirectToAction("Index");
                }
            }

        }
        [HttpGet]
        public IActionResult Checkout()
        {
            IEnumerable<Checkout> checkouts = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/AddToCart/Checkout");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cart>>(readTask.Result);
                    readTask.Wait();
                    checkouts = (IEnumerable<Checkout>?)deserialized;
                }
                else
                {
                    checkouts = Enumerable.Empty<Checkout>();
                    ModelState.AddModelError(string.Empty, "Cart not found.");
                }
            }
            return View(checkouts);
        }
    }
}
