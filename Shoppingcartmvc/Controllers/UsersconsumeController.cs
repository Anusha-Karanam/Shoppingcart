using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoppingcartmvc.Models;
using System.Text;

namespace Shoppingcartmvc.Controllers
{
    public class UsersconsumeController : Controller
    {
        Uri baseaddress = new Uri("https://localhost:7185/api");
        private readonly HttpClient _client;

        public UsersconsumeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseaddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<User> userList = new List<User>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Userss/GetUsers").Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userList = JsonConvert.DeserializeObject<List<User>>(data);
            }

            return View(userList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Userss/PostUser", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "User Created";
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"/Userss/GetUser/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error
                var data = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(data);
                return View(user);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"/Userss/PutUser/{model.UserId}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "User details updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update user details";
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
        public IActionResult Delete(int id)
        {
            try
            {
                User user = new User();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/Userss/GetUser/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<User>(data);
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        //[HttpDelete]
        
        public async Task<IActionResult> UserExists(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + $"/Userss/DeleteUser/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "User details deleted";
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    TempData["errorMessage"] = "Failed to delete user details";
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

