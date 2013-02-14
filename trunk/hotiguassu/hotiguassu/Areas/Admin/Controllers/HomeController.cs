using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotiguassu.Areas.Admin.Controllers
{
    public class HomeController : Controller 
    {
        public ActionResult Index()
        {
            var login = this.User.Identity.Name;
            var usuario = "teste";
            
            //  var usuario = db.Usuarios.Include("perfil");
            if (usuario.ToString() != "") {
            //if ((usuario.ToString() != "") && (login.ToString() != ""))
            //{
            //    foreach (var item in usuario)
            //    {
            //        if ((item.dsLogin == login) && ((item.perfil.nmPerfil == "Hoteleiro") || (item.perfil.nmPerfil == "Hoteleiro Pendente")))
            //        {
            //            var usuarioPermissao = from p in item.perfil.perfilTelas select p;

            //            Session["permissao"] = usuarioPermissao.ToList();

            //            Session["grupo"] = (from g in usuarioPermissao select g.tela.nmGrupo).Distinct().ToList();

            //        }
            //    }
            //    ViewBag.ListaNovidades = (from s in db.Novidade
            //                              where s.flSituacao == true
            //                              orderby s.idNovidade descending
            //                              select s).Take(4).ToList();

                return RedirectToAction("Index", "../Admin/Autenticacao/");

            } else {

                return RedirectToAction("Index", "../Admin/Autenticacao/");

            }
        }

    }
}
