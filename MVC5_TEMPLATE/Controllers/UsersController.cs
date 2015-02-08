using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrien.Template.Models;
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
    }
}