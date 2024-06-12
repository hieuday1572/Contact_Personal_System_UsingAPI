using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        public UserDto login { get; set; }
        public LoginModel()
        {
            _client = new HttpClient();
        }
        public async Task<IActionResult> OnGet(bool logout)
        {
            if (logout == true)
            {
                HttpContext.Session.Clear();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
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
                return RedirectToAction("Index", "Home");
            }
            return Page();
        }
    }
}
