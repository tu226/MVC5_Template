using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrien.Template.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Adrien.Template.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Users() {

            return Json(UserModel.GetUsers(),JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(UserModel user) {

            user.Save();
            return Json("",JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register(RegisterBindingModel model) {

            ApplicationUserManager usermanager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new User() { UserName = model.username };
            IdentityResult result = usermanager.Create(user, model.Password);
            if (!result.Succeeded) {
                return new HttpStatusCodeResult(500);
            }
            return Json(UserModel.GetUsers());
            
        }
    }
}