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
        [HttpPost]
        public async Task<IActionResult> CreateLabel(string LabelName)
        {
            LabelDto Label = new LabelDto()
            {
                Id = default(int),
                LabelName = LabelName,
                UserId = HttpContext.Session.GetInt32("userId")
            };
            await _httpClient.PostAsJsonAsync("https://localhost:7258/api/Label/CreateLabel", Label);
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7258/api/Label/GetLabels");
            string strData = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("labels", strData);
            List<LabelDto> list = JsonConvert.DeserializeObject<List<LabelDto>>(strData);
            int id = list.Max(p => p.Id);
            var labelHtml = $"<li class=\"nav-item\">\r\n<a class=\"nav-link\" href=\"tables.html\">\r\n<i class=\"bi bi-bookmark-fill\"></i>\r\n<span>{LabelName}</span>\r\n</a>\r\n</li>";
            return Json(new { success = true, labelHtml });
        }
    }
}
