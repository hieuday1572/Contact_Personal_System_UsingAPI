using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class TrashController : Controller
    {
        private readonly HttpClient _httpClient;
        public TrashController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetContacts/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ContactDto>>(strData);
            list = list.Where(p => p.IsInTrash == true).ToList();           
            return View(list);
        }

        public async Task<IActionResult> HandleRequest(int id,bool restore, bool delete)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetContactById/{id}");           
            if (restore == true)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ContactDto>(strData);
                value.IsInTrash = false;
                await _httpClient.PutAsJsonAsync($"https://localhost:7258/api/Contact/UpdateContact", value);
                TempData["restore"] = "Restore successfully";
                return RedirectToAction(nameof(Index));
            }
            else if(delete == true)
            {
                await _httpClient.DeleteAsync($"https://localhost:7258/api/Contact/DeleteContact/{id}");
                TempData["delete"] = "Delete successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
