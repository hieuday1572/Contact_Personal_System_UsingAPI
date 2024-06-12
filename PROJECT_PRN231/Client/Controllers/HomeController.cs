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

        public async  Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("userId")==null)
            {
                return RedirectToPage("/Login");
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
            response = await _client.GetAsync("https://localhost:7258/api/Label/GetLabels");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                strData = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("labels",strData);
            }
            return View(list);
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
