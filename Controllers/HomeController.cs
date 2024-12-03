using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace form_authentication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            Debug.WriteLine($"Home/Home - User Identity: {User.Identity.Name}, IsAuthenticated: {User.Identity.IsAuthenticated}");
            return View();
        }
    }
}