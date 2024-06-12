using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    [BindProperties]
    public class ProfileModel : PageModel
    {
        private readonly HttpClient _client;
        public UserDto user { get; set; }
        public ProfileModel()
        {
            _client = new HttpClient();
        }
        public async Task<IActionResult> OnGet()
        {
            var id = HttpContext.Session.GetInt32("userId");
            var name = HttpContext.Session.GetString("username");
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7258/api/User/GetByName/{name}");
            string strData = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["Error"] = "Not Found !";
                return Page();
            }
            user = JsonConvert.DeserializeObject<UserDto>(strData);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            HttpResponseMessage response = await _client.GetAsync("https://localhost:7258/api/User/GetUsers");
            string strData = await response.Content.ReadAsStringAsync();
            var listUser = JsonConvert.DeserializeObject<List<UserDto>>(strData);
            if(listUser.Any(p => p.Username.Equals(user.Username) && p.Id != user.Id))
            {
                ModelState.AddModelError("user.Username", "Username already existed !");
            }
            if (ModelState.IsValid)
            {
                response = await _client.PutAsJsonAsync("https://localhost:7258/api/User/UpdateUser",user);
                strData = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorUpdate"] = strData;
                    return Page();
                }
                TempData["success"] = "Edited successfully !";
            }
            return Page();
        }
    }
}
