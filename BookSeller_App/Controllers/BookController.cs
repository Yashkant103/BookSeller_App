using BookSeller_App.BAL;
using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookSeller_App.Controllers
{
    [CheckAccess]
    public class BookController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:2482/api");
        private readonly HttpClient _httpClient;

        public BookController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            ViewBag.Genre = Type_Combobox();
            List<BookModel> Book = new List<BookModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Books/Getall").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<List<BookModel>>(data);
                //var dataobj = json.data;
                //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                //author = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                return View("GetAllBooks", json);
            }
            return View("GetAllBooks");
        }
        [HttpGet]
        public IActionResult Delete(int BookID)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"{baseAddress}/Books/Delete?BookID=" + BookID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "Book Delete Successfully";
            }
            return RedirectToAction("GetAllBooks");
        }
        public IActionResult AddBooks()
        {
            ViewBag.Genre = Type_Combobox();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int BookID)
        {
            ViewBag.Genre = Type_Combobox();
            BookModel BookModel = new BookModel();
            HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/Books/GetById/" + BookID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                BookModel json = JsonConvert.DeserializeObject<BookModel>(data) as BookModel;
                return View("AddBooks", json);
            }
            return View("GetAllBooks");

        }

        [HttpPost]
        public async Task<IActionResult> Post(BookModel BookModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(BookModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                /* if (BookModel.BookID != null || BookModel.BookID > 0)
                 {
                     ViewBag.Action = "Edit";
                 }
                 ViewBag.Action = "Add";*/
                if (BookModel.BookID == 0)
                {
                    HttpResponseMessage response = _httpClient.PostAsync($"{baseAddress}/Books/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "New Book Insrted Sucessfully";
                        return RedirectToAction("GetAllBooks");
                    }
                    TempData.Clear();
                }
                else
                {
                    HttpResponseMessage response = await _httpClient.PutAsync($"{baseAddress}/Books/Update/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Book Updated Successfully";
                        return RedirectToAction("GetAllBooks");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllBooks");
        }

        public List<GenreModel> Type_Combobox()
        {
            List<GenreModel> genreModels = new List<GenreModel>();
            HttpResponseMessage r = _httpClient.GetAsync($"{baseAddress}/BookType/Getall/").Result;
            if (r.IsSuccessStatusCode)
            {
                string data = r.Content.ReadAsStringAsync().Result;
                genreModels = JsonConvert.DeserializeObject<List<GenreModel>>(data);
            }
            List<GenreModel> Type_DropDowns = new List<GenreModel>();
            foreach (var item in genreModels)
            {

                GenreModel genreList = new GenreModel();
                genreList.GenreID = Convert.ToInt32(item.GenreID);
                genreList.GenreName = Convert.ToString(item.GenreName);
                Type_DropDowns.Add(genreList);
            }
            return Type_DropDowns;
        }

    }
}