using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrien.Template.Models;

namespace Adrien.Template.Controllers
{
    [Authorize(Roles="admin")]
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Roles() {

            return Json(RoleModel.GetRoles(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(RoleModel role) {
            role.Save();
            return Json(role,JsonRequestBehavior.AllowGet); 
        }

        
    }
}