using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrien.Template.Models;


namespace Adrien.Template.Controllers
{
    [Authorize]
    public class AccountAdministrationController : Controller
    {
        
        public ActionResult Index()
        {
            AccountsAdministationViewModel model = new AccountsAdministationViewModel();
            return View(model);
        }

        public ActionResult EditUser(int userid) {

            EditUserControllerViewModel model = new EditUserControllerViewModel(userid);
            return View(model);
        
        }
    }
}