using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookSeller_App.Controllers
{
	public class UserController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:2482/api");
		private readonly HttpClient _httpClient;

		public UserController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}

		[HttpGet]
		public IActionResult GetAllUsers()
		{
			List<UserModel> users = new List<UserModel>();
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/User/Get").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic json = JsonConvert.DeserializeObject(data);
				var dataOfObject = json.data;
				var extractedDataJson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
				users = JsonConvert.DeserializeObject<List<UserModel>>(extractedDataJson);
			}
			return View("GetAllUsers", users);
		}
	}
}
