using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hotiguassu.Models;
using System.Web.Security;

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
        public ActionResult LogOn(UsuarioModels usuario)
        {
            var q = from u in db.UsuarioModels
                    where u.Login == usuario.Login && u.Senha == usuario.Senha
                    select u;

            var usu = q.FirstOrDefault();

            if (usu != null)
            {
                var logado = true;

                if (logado)
                {
                    FormsAuthentication.SetAuthCookie(usu.Login, true);
                    return RedirectToAction("Admin", "");
                }
                else
                {
                    ModelState.AddModelError("", "Usuário ou senha incorretos.");
                }

            }
                 return View("LogOn");
            }

      



    }
}
