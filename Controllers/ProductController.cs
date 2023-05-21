using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCartTest.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShoppingCartTest.Controllers
{

    public class ProductController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                var responseTask = await client.GetAsync("api/Product/GetAll");

                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(readTask.Result);
                    readTask.Wait();
                    products = deserialized;
                }
                else
                {
                    products = Enumerable.Empty<Product>();
                    ModelState.AddModelError(string.Empty, "Product not found.");
                }
            }
            return View(products);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product) {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7253/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var postTask =await client.PostAsJsonAsync<ProductModel>("api/Product", product);
            
            
            if (postTask.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(product);
        }
        
       public async Task<IActionResult> UpdateProduct(int id)
        {

            Product product = new Product();
            using (var client = new HttpClient())
            {

                
                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                using (var responce = await client.GetAsync($"api/Cart/GetById/{id}"))
                {
                    string readTask = await responce.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(readTask);
                }
            }

            return View(product);

       }
        [HttpPost]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
                var client = new HttpClient();
            
                client.BaseAddress = new Uri("https://localhost:7253/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask =await client.PutAsJsonAsync<Product>("api/Product/Update", product);
            if (postTask.IsSuccessStatusCode)
                return RedirectToAction("Index");
            else
            {


                ModelState.AddModelError(string.Empty, "Failed Try again.");
                return View(product);
            }
        }

        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeletData(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7253/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));


                    var postTask = await client.PostAsJsonAsync("api/Cart/Delete", id);
                    var result = await postTask.Content.ReadAsStringAsync();
                    if (postTask.IsSuccessStatusCode)
                    {
                        return Json(result);
                    }
                }
                return Json("\"Failed Try again.\"");
            }
            catch (Exception ex) { throw ex; }
        }


    }
        

}

    
    

