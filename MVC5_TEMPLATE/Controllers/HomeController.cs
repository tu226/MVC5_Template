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

    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(string username, string password, string returnUrl)
        {
            if (Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated) {
                return RedirectToAction("Index");
            }
            if (!string.IsNullOrEmpty(username)  && !string.IsNullOrEmpty(password))
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

                    if (string.IsNullOrEmpty(returnUrl)) { returnUrl = "/home"; };
                    return Redirect(returnUrl);
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
