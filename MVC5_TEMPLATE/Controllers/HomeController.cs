using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Adrien.Template.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles="guest")]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> Login(string username, string password, string returnUrl)
        {
            if (username != null && username !=null)
            {

                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                User user = await userManager.FindAsync(username, password);
                if (user != null)
                {

                    IList<string> roles = await userManager.GetRolesAsync(user.Id);
                    List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
                    foreach (string role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    ClaimsIdentity cookiesIdentity = new ClaimsIdentity(claims.ToArray(), CookieAuthenticationDefaults.AuthenticationType);
                    Request.GetOwinContext().Authentication.SignIn(cookiesIdentity);

                    return Redirect("returnUrl");
                }
                else {
                    ViewBag.ErrorMessage = "Incorrect credentials";
                }
            }
            return View();

        }

        public ActionResult Logout()
        {

             Request.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("LogIn");

        }

    }
}
