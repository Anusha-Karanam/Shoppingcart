using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoppingcartuser.Models;
using System.Diagnostics;

namespace Shoppingcartuser.Controllers
{
    public class HomeController : Controller
    {
        Uri baseaddress = new Uri("https://localhost:7185/api");
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
