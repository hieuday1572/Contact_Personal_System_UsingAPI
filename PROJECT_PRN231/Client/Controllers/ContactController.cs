using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;
        public ContactController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            ContactDto contact = new ContactDto();
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactDto contact)
        {
            if (ModelState.IsValid)
            {
                contact.UserId = HttpContext.Session.GetInt32("userId");
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7258/api/Contact/CreateContact", contact);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetContactById/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["Error"] = "Not Found !";
            }
            else
            {
                string strData = await response.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<ContactDto>(strData);
                return View(detail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(ContactDto contact)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"https://localhost:7258/api/Contact/UpdateContact",contact);
                TempData["success"] = "Update successfully";
                return RedirectToAction(nameof(Details));
            }
            else
            {
                ViewData["ErrorUpdate"] = "Error Update";
            }           
            return View(contact);
        }

        public async Task<IActionResult> PutToTrash(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetContactById/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<ContactDto>(strData);
                contact.IsInTrash= true;
                await _httpClient.PutAsJsonAsync("https://localhost:7258/api/Contact/UpdateContact", contact);
            }
            return RedirectToAction("Index","Home");
        }
    }
}
