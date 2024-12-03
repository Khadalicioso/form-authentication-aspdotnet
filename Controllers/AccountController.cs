using form_authentication.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;

namespace form_authentication.Controllers
{
    public class AccountController : Controller
    {
        private UserDAL _userDAL = new UserDAL();

        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            try
            {
                string username = form["Username"];
                string password = form["Password"];

                Debug.WriteLine($"Login attempt - Username: {username}, Password: {password}");

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View();
                }

                var user = _userDAL.GetUserByUsername(username);
                Debug.WriteLine($"User found in database: {user != null}");

                if (user == null)
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View();
                }

                Debug.WriteLine($"Stored password hash: {user.PasswordHash}");
                Debug.WriteLine($"Comparing entered password: {password} with stored hash: {user.PasswordHash}");

                if (password == user.PasswordHash)
                {
                    Debug.WriteLine("Password matches! Setting auth cookie...");
                    FormsAuthentication.SetAuthCookie(username, false);
                    Debug.WriteLine("Auth cookie set, redirecting to Home/Index");
                    return RedirectToAction("Home", "Home");
                }

                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        // GET: Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}