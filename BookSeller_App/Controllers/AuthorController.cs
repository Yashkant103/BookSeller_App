using BookSeller_App.BAL;
using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookSeller_App.Controllers
{
    [CheckAccess]
    public class AuthorController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:2482/api");
        private readonly HttpClient _httpClient;

        public AuthorController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            List<AuthorModel> author = new List<AuthorModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Author/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                IEnumerable<AuthorModel> json = JsonConvert.DeserializeObject<List<AuthorModel>>(data);
                //var dataobj = json.data;
                //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                //author = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                return View("GetAllAuthors", json);
            }
            return View("GetAllAuthors");
        }
        public IActionResult DeleteAuthor(int AuthorID)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"{baseAddress}/Author/Delete?AuthorID=" + AuthorID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "Author Delete Successfully";
            }
            return RedirectToAction("GetAllAuthors");
        }


        [HttpGet]
        public IActionResult Edit(int AuthorID)
        {
            AuthorModel AuthorModel = new AuthorModel();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Author/GetbyId/" + AuthorID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                AuthorModel json = JsonConvert.DeserializeObject<AuthorModel>(data) as AuthorModel;
                return View("AddAuhtor", json);
            }
            return View("GetAllAuthors");

        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthorModel AuthorModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(AuthorModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                if (AuthorModel.AuthorID != null || AuthorModel.AuthorID > 0)
                {
                    ViewBag.Action = "Edit";
                }
                ViewBag.Action = "Add";
                if (AuthorModel.AuthorID == 0)
                {
                    HttpResponseMessage response = _httpClient.PostAsync($"{baseAddress}/Author/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Author insert Successfully";
                        return RedirectToAction("GetAllAuthors");
                    }
                    TempData.Clear();

                }
                else
                {
                    HttpResponseMessage response = await _httpClient.PutAsync($"{baseAddress}/Author/Put/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Auhtor Updated Successfully";
                        return RedirectToAction("GetAllAuthors");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllAuthors");
        }
        public IActionResult AddAuhtor()
        {
            return View();
        }
    }
}
