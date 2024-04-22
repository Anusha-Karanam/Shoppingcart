using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoppingcartmvc.Models;
using System.Text;


namespace Shoppingcartmvc.Controllers
{
    public class ProductconsumeController : Controller
    {
        Uri baseaddress = new Uri("https://localhost:7185/api");
        private readonly HttpClient _client;
        public ProductconsumeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseaddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Product> productList = new List<Product>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Products/GetAllProducts").Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<Product>>(data);
            }

            return View(productList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Products/PostProduct", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Prodcut Created";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"/Products/GetProductById/{Id}");
                response.EnsureSuccessStatusCode(); // Throw on error
                var data = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(data);
                return View(product);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"/Products/PutProduct/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product details updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update details details";
                    return View(model);
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                Product product = new Product();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Products/GetProductById/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(data);
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        public async Task<IActionResult> ProductExists(int Id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + $"/Products/DeleteProduct/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product details deleted";
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["errorMessage"] = "Failed to delete product details";
                    return RedirectToAction("Index");
                }



            }
            catch (HttpRequestException ex)
            {

                TempData["errorMessage"] = "Failed to connect to server: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
