using BookSeller_App.DAL;
using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookSeller_App.Controllers
{
	public class LoginController : Controller
	{
		DAL_Helper dAL_Helper = new DAL_Helper();
		[Route("/Login")]

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoginCheck(LoginModel model)
		{
			string ApiUrl = $"/Login/{model.UserName}/{model.UserPassword}";
			var apiResponse = await dAL_Helper.SendRequestAsync<object>(ApiUrl, HttpMethod.Get);

			if (apiResponse.IsSuccess)
			{
				return RedirectToAction("Index", "Home");
			}
			@ViewData["Message"] = apiResponse.Message;
			@ViewData["ErrorMessage"] = apiResponse.ErrorMessage;
			return View();
		}
	}
}
