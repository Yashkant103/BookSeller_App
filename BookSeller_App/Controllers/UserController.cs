using BookSeller_App.BAL;
using BookSeller_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region get all users
        [HttpGet]
        [CheckAccess]
        public IActionResult GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/User/Get").Result;
            ViewBag.RoleList = dropDowns();
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
        #endregion

        #region Delete Users
        [HttpGet]
        public IActionResult Delete(int UserID)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = _httpClient.DeleteAsync($"{baseAddress}/User/Delete?UserID=" + UserID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "User Deleted Successfully";
            }
            return RedirectToAction("GetAllUsers");
        }
        #endregion

        #region Insert User
        [HttpGet]
        //public IActionResult Edit(int UserID)
        //{
        //    UserModel userModel = new UserModel();
        //    HttpResponseMessage response = _httpClient.GetAsync($"{baseAddress}/User/GetById/" + UserID).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        dynamic json = JsonConvert.DeserializeObject(data);
        //        var dataobj = json.data;
        //        var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
        //        userModel = JsonConvert.DeserializeObject<UserModel>(extractDatajosn);
        //    }
        //    return View("RegisterUsers", userModel);
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(int UserID)
        {
            UserModel userModel = new UserModel();
            HttpResponseMessage response = await _httpClient.GetAsync($"{baseAddress}/User/GetById/" + UserID);
            ViewBag.RoleList = dropDowns();
            //ViewBag.Message = "Edit User";

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(data);
                var dataobj = json.data;
                var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                userModel = JsonConvert.DeserializeObject<UserModel>(extractDatajosn);
            }
            return View("RegisterUsers", userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserModel userModel)
        {
            try
            {

                string data = JsonConvert.SerializeObject(userModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //ViewBag.Message = "Register User";
                if (userModel.UserID == 0)
                {
                    HttpResponseMessage response = _httpClient.PostAsync($"{baseAddress}/User/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "User insert Successfully";
                        return RedirectToAction("GetAllUsers");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _httpClient.PutAsync($"{baseAddress}/User/Put/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "User Updated Successfully";
                        return RedirectToAction("GetAllUsers");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllUsers");
        }

        public IActionResult RegisterUsers()
        {
            ViewBag.RoleList = dropDowns();
            return View();
        }
        public List<Role_DropDown> dropDowns()
        {
            List<Role_DropDown> role = new List<Role_DropDown>();
            HttpResponseMessage r = _httpClient.GetAsync($"{baseAddress}/UserRole/Get").Result;
            if (r.IsSuccessStatusCode)
            {
                string data = r.Content.ReadAsStringAsync().Result;
                role = JsonConvert.DeserializeObject<List<Role_DropDown>>(data);
            }
            List<Role_DropDown> role_DropDowns = new List<Role_DropDown>();

            foreach (var item in role)
            {

                Role_DropDown list = new Role_DropDown();
                list.RoleID = Convert.ToInt32(item.RoleID);
                list.RoleName = Convert.ToString(item.RoleName);
                role_DropDowns.Add(list);
            }
            return role_DropDowns;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> checkLoginDetails(LoginModel user)
        {
            if (!TryValidateModel(user)) return View("Login", user);
            int id = await Login(user);
            if (!(user != null && id > 0))
            {
                TempData["error"] = "Username or Password is incorrect";
                return View("Login", user);
            }
            _SetSession(id, user);
            int? a = UserSessionCV.UserId();
            TempData.Clear();
            if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("UserPassword") != null)
                return RedirectToAction("Index", "Home");
            return View("Login");
        }
        private void _SetSession(int id, LoginModel user)
        {
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserPassword", user!.UserPassword);
            HttpContext.Session.SetInt32("UserID", id);

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [HttpPost]
        public async Task<int> Login(LoginModel loginModel)
        {
            try
            {

                string data = JsonConvert.SerializeObject(loginModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{baseAddress}/User/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Masssege"] = "User login Successfully";
                    string userid = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(userid);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return -1;
        }
    }
}
