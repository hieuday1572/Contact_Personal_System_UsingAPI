using Client.Helpper;
using Client.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;

namespace Client.Controllers
{
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;
        public ContactController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult CreateContact()
        {
            ContactDto contact = new ContactDto();
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(ContactDto contact, IFormFile? fThumb, string[] Email, string[] Phone)
        {
            if (ModelState.IsValid)
            {
                List<string> cleanedList = new List<string>(Email); // Chuyển mảng thành List để sử dụng RemoveAll()

                cleanedList.RemoveAll(string.IsNullOrEmpty);
                string emailList = JsonConvert.SerializeObject(cleanedList);

                List<string> cleanedPhoneList = new List<string>(Phone); // Chuyển mảng thành List để sử dụng RemoveAll()

                cleanedPhoneList.RemoveAll(string.IsNullOrEmpty);
                string phoneList = JsonConvert.SerializeObject(cleanedPhoneList);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(fThumb.FileName) + extension;
                    contact.Image = await Utilities.UploadFile(fThumb, @"ContactImages", image.ToLower());
                }
                contact.UserId = HttpContext.Session.GetInt32("userId");
                contact.Email = emailList;
                contact.Phone = phoneList;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7258/api/Contact/CreateContact", contact);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, bool visit)
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
                string[] listEmail = JsonConvert.DeserializeObject<string[]>(detail.Email);
                string[] listPhone = JsonConvert.DeserializeObject<string[]>(detail.Phone);
                ViewData["listEmail"] = listEmail;
                ViewData["listPhone"] = listPhone;
                if (visit == true)
                {
                    if (detail.VisitedCount == null)
                    {
                        detail.VisitedCount = 0;
                    }
                    detail.VisitedCount += 1;
                    await _httpClient.PutAsJsonAsync($"https://localhost:7258/api/Contact/UpdateContact", detail);
                }
                response = await _httpClient.GetAsync("https://localhost:7258/api/Label/GetLabels");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    strData = await response.Content.ReadAsStringAsync();
                    var labels = JsonConvert.DeserializeObject<List<LabelDto>>(strData);
                    labels = labels.Where(p => p.UserId == HttpContext.Session.GetInt32("userId")).ToList();
                    response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetLabelsByContact/{id}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        strData = await response.Content.ReadAsStringAsync();
                        List<LabelDto>? labelsContact = JsonConvert.DeserializeObject<List<LabelDto>>(strData);
                        ViewData["labelsContact"] = labelsContact;
                        foreach (var item in labelsContact)
                        {
                            if (labels.Any(p => p.Id == item.Id))
                            {
                                var remove = labels.FirstOrDefault(p => p.Id.Equals(item.Id));
                                labels.Remove(remove);
                            }
                        }

                    }
                    ViewData["labels"] = labels;
                }
                return View(detail);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(ContactDto contact, IFormFile? fThumb, string[] Email, string[] Phone)
        {
            if (ModelState.IsValid)
            {
                List<string> cleanedList = new List<string>(Email); // Chuyển mảng thành List để sử dụng RemoveAll()

                cleanedList.RemoveAll(string.IsNullOrEmpty);
                string emailList = JsonConvert.SerializeObject(cleanedList);

                List<string> cleanedPhoneList = new List<string>(Phone); // Chuyển mảng thành List để sử dụng RemoveAll()

                cleanedPhoneList.RemoveAll(string.IsNullOrEmpty);
                string phoneList = JsonConvert.SerializeObject(cleanedPhoneList);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(fThumb.FileName) + extension;
                    contact.Image = await Utilities.UploadFile(fThumb, @"ContactImages", image.ToLower());
                }
                contact.Email = emailList;
                contact.Phone = phoneList;
                contact.ModifiedDate = DateTime.Now;
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"https://localhost:7258/api/Contact/UpdateContact", contact);
                TempData["success"] = "Update successfully";
                return RedirectToAction(nameof(Details));
            }
            else
            {
                TempData["ErrorUpdate"] = "Error Update";
            }
            return View(contact);
        }

        public async Task<IActionResult> AddLabel(int labelId, int contactId)
        {
            if (labelId != null && contactId != null)
            {
                Contact_LabelDto ct_lb = new Contact_LabelDto()
                {
                    ContactId = contactId,
                    LabelId = labelId
                };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"https://localhost:7258/api/ContactLabel/CreateContactLabel", ct_lb);
            }
            return RedirectToAction(nameof(Details), new { id = contactId });
        }

        public async Task<IActionResult> RemoveLabel(int labelId, int contactId, bool labelPage)
        {
            if (labelId != null && contactId != null)
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7258/api/ContactLabel/DeleteLabel/{contactId}/{labelId}");
            }
            if (labelPage == true)
            {
                return RedirectToAction("Index", "Label", new { id = labelId });
            }
            else
            {
                return RedirectToAction(nameof(Details), new { id = contactId });
            }
        }
        public async Task<IActionResult> PutToTrash(int id, bool? labelPage)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetContactById/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<ContactDto>(strData);
                contact.IsInTrash = true;
                contact.TrashDate = DateTime.Now.AddMinutes(1);
                await _httpClient.PutAsJsonAsync("https://localhost:7258/api/Contact/UpdateContact", contact);
            }
            if (labelPage == true)
            {
                return RedirectToAction("Index", "Label");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> PopularContact()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7258/api/Contact/GetPopularContacts/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ContactDto>>(strData);
            list = list.Where(p => p.IsInTrash == false).ToList();
            return View(list);
        }
    }
}
