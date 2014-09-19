using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hotiguassu.Models;

namespace hotiguassu.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private hotiguassuContext db = new hotiguassuContext();


        protected override void OnException(ExceptionContext filterContext)
        {
            ModuloGeral.WriteLog(Server.MapPath("~/ErrorLog/log_site.txt"), filterContext.Exception.ToString());

            // Output a nice error page
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                // this.View("Error",filterContext.Exception ).ExecuteResult(this.ControllerContext);
            }
        }

        public ActionResult Index()
        {

            var login = this.User.Identity.Name;

            if (login.ToString() != "")
            {
                if ("Administrador" == login.ToString())
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Autenticacao");
                }
            }
            return RedirectToAction("Index", "Autenticacao");

        }

    }
}
