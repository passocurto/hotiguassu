using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hotiguassu.Models;
using System.Web.Security;
using System.Configuration;

namespace hotiguassu.Areas.Admin.Controllers
{
    public class AutenticacaoController : Controller
    {

        private hotiguassuContext db = new hotiguassuContext();

        public ActionResult Index()
        {
            return View("LogOn");
        }

        [HttpPost]
        public ActionResult Index(UsuarioModels usuario)
        {
            var usuLogin = ConfigurationManager.AppSettings["Login"].ToString();
            var usuSenha = ConfigurationManager.AppSettings["Senha"].ToString();
            if (Request.Form["login"].Equals(usuLogin) && Request.Form["senha"].Equals(usuSenha))
            {
                FormsAuthentication.SetAuthCookie("Administrador", true);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("", "Usuário ou senha incorretos.");
            }

            return View("LogOn");
        }
    }
}
