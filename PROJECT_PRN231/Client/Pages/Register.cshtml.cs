using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Client.Pages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _client;
        public UserDto register { get; set; }
        public RegisterModel()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(string confirmPassword)
        {
            if(confirmPassword == null || !confirmPassword.Equals(register.Password)) 
            {
                ModelState.AddModelError("register.Password","Password does not match to confirm password");
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
                    return RedirectToAction("Index", "Home");
                }
            }           
            return Page();
        }
    }
}
