using Client.Helpper;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http.Headers;

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

        public async Task<IActionResult> Index(string search)
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
            list = list.Where(p => p.IsInTrash == false).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                list = list.Where(p => p.FullName.ToLower().Contains(search.ToLower())).ToList();
                ViewData["search"] = search;
            }
            response = await _client.GetAsync("https://localhost:7258/api/Label/GetLabels");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                strData = await response.Content.ReadAsStringAsync();
                var labels = JsonConvert.DeserializeObject<List<LabelDto>>(strData);
                labels = labels.Where(p => p.UserId == id).ToList();
                strData = JsonConvert.SerializeObject(labels);
                HttpContext.Session.SetString("labels", strData);
            }
            else
            {
                HttpContext.Session.SetString("labels", "[]");
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
                //Test authentication sử dụng jwt bằng cách chạy debug để kiểm tra giá trị trả về trong 2 TH khi có token trong HTTP và khi không có.
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
                response = await _client.GetAsync($"https://localhost:7258/api/User/GetByName/{login.Username}");
                strData = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDto>(strData);
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("image", user.Image);

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
             return RedirectToAction("Login",new{ logout = true });
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
        public async Task<IActionResult> Profile(UserDto user, IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(fThumb.FileName) + extension;
                    user.Image = await Utilities.UploadFile(fThumb, @"UserImages", image.ToLower());
                }
                HttpResponseMessage response = await _client.PutAsJsonAsync("https://localhost:7258/api/User/UpdateUser", user);
                string strData = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorUpdate"] = strData;
                    return View(user);
                }
                HttpContext.Session.SetString("image", user.Image);
                TempData["success"] = "Edited successfully !";
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
        }

        public async Task<JsonResult> GetSearchValue(string search)
        {
            int id = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7258/api/Contact/GetContacts/{id}?$filter=contains(tolower(FullName),'{search.ToLower()}')");
            string strData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ContactDto>>(strData);
            list = list.Where(p => p.IsInTrash == false).ToList();
            return new JsonResult(list);
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

    public class ContactSearchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
