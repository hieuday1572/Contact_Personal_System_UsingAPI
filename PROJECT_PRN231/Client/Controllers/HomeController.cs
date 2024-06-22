using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7258/api/Contact/GetContacts/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ContactDto>>(strData);
            list=list.Where(p => p.IsInTrash==false).ToList();
            response = await _client.GetAsync("https://localhost:7258/api/Label/GetLabels");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                strData = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("labels", strData);
            }
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Login(bool logout)
        {
            if (logout == true)
            {
                HttpContext.Session.Clear();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto login)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("https://localhost:7258/api/Auth/login", login);
            string strData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["fail"] = strData;
            }
            else
            {
                response = await _client.GetAsync($"https://localhost:7258/api/User/GetByName/{login.Username}");
                strData = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<UserDto>(strData);
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("username", user.Username);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string confirmPassword, UserDto register)
        {
            if (confirmPassword == null || !confirmPassword.Equals(register.Password))
            {
                ModelState.AddModelError("register.Password", "Password does not match to confirm password");
            }
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("https://localhost:7258/api/Auth/register", register);
                string strData = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    TempData["fail"] = strData;
                }
                else
                {
                    response = await _client.GetAsync($"https://localhost:7258/api/User/GetByName/{register.Username}");
                    strData = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<UserDto>(strData);
                    HttpContext.Session.SetInt32("userId", user.Id);
                    HttpContext.Session.SetString("username", user.Username);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var id = HttpContext.Session.GetInt32("userId");
            var name = HttpContext.Session.GetString("username");
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7258/api/User/GetByName/{name}");
            string strData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["Error"] = "Not Found !";
                return View();
            }
            var user = JsonConvert.DeserializeObject<UserDto>(strData);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserDto user)
        {
            HttpResponseMessage response = await _client.GetAsync("https://localhost:7258/api/User/GetUsers");
            string strData = await response.Content.ReadAsStringAsync();
            var listUser = JsonConvert.DeserializeObject<List<UserDto>>(strData);
            if (listUser.Any(p => p.Username.Equals(user.Username) && p.Id != user.Id))
            {
                ModelState.AddModelError("user.Username", "Username already existed !");
            }
            if (ModelState.IsValid)
            {
                response = await _client.PutAsJsonAsync("https://localhost:7258/api/User/UpdateUser", user);
                strData = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorUpdate"] = strData;  
                    return View(user);
                }
                TempData["success"] = "Edited successfully !";
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
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
