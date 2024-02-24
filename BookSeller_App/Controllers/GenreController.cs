using BookSeller_App.BAL;
using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookSeller_App.Controllers
{
    [CheckAccess]
    public class GenreController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:2482/api");
        private readonly HttpClient _httpClient;

        public GenreController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            List<GenreModel> Genre = new List<GenreModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Genre/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                IEnumerable<GenreModel> json = JsonConvert.DeserializeObject<List<GenreModel>>(data);
                //var dataobj = json.data;
                //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                //Genre = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                return View("GetAllGenres", json);
            }
            return View("GetAllGenres");
        }
        public IActionResult DeleteGenre(int GenreID)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"{baseAddress}/Genre/Delete?GenreID=" + GenreID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "Genre Delete Successfully";
            }
            return RedirectToAction("GetAllGenres");
        }


        [HttpGet]
        public IActionResult Edit(int GenreID)
        {
            GenreModel GenreModel = new GenreModel();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Genre/GetbyId/" + GenreID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                GenreModel json = JsonConvert.DeserializeObject<GenreModel>(data) as GenreModel;
                return View("AddGenre", json);
            }
            return View("GetAllGenres");

        }

        [HttpPost]
        public async Task<IActionResult> Post(GenreModel GenreModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(GenreModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                if (GenreModel.GenreID != null || GenreModel.GenreID > 0)
                {
                    ViewBag.Action = "Edit";
                }
                ViewBag.Action = "Add";
                if (GenreModel.GenreID == 0)
                {
                    HttpResponseMessage response = _httpClient.PostAsync($"{baseAddress}/Genre/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Genre insert Successfully";
                        return RedirectToAction("GetAllGenres");
                    }
                    TempData.Clear();

                }
                else
                {
                    HttpResponseMessage response = await _httpClient.PutAsync($"{baseAddress}/Genre/Put/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Auhtor Updated Successfully";
                        return RedirectToAction("GetAllGenres");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllGenres");
        }
        public IActionResult AddGenre()
        {
            return View();
        }
    }
}
