using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class LabelController : Controller
    {
        private readonly HttpClient _httpClient;
        public LabelController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Label/GetLabelById/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["Not Found"] = "Not Found !";
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            LabelDto label = JsonConvert.DeserializeObject<LabelDto>(strData);
            ViewData["label"] = label;
            response = await _httpClient.GetAsync($"https://localhost:7258/api/Label/GetContactByLabel/{id}");
            strData = await response.Content.ReadAsStringAsync();
            List<ContactDto> listContact = JsonConvert.DeserializeObject<List<ContactDto>>(strData);
            return View(listContact);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLabel(string LabelName)
        {
            LabelDto Label = new LabelDto()
            {
                Id = default,
                LabelName = LabelName,
                UserId = HttpContext.Session.GetInt32("userId")
            };
            await _httpClient.PostAsJsonAsync("https://localhost:7258/api/Label/CreateLabel", Label);
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7258/api/Label/GetLabels");
            string strData = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("labels", strData);
            List<LabelDto> list = JsonConvert.DeserializeObject<List<LabelDto>>(strData);
            int id = list.Max(p => p.Id);
            var labelHtml = $"<li class=\"nav-item\">\r\n<a class=\"nav-link\" asp-controller=\"Label\" asp-action=\"Index\" asp-route-id=\"{id}\">\r\n<i class=\"bi bi-bookmark-fill\"></i>\r\n<span>{LabelName}</span>\r\n</a>\r\n</li>";
            return Json(new { success = true, labelHtml });
        }
    }
}
