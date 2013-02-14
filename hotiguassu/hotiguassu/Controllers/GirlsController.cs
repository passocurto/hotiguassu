using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hotiguassu.Models;
using System.Web.Security;

namespace hotiguassu.Controllers
{ 
    public class GirlsController : Controller
    {
        private hotiguassuContext db = new hotiguassuContext();


        //[HttpPost]
        //public ActionResult LogOn(UsuarioModels model, string returnUrl)
        //{
        //    var q = from u in db.UsuarioModels
        //            where u.UserName == model.UserName
        //            select u;

        //  var usu = q.FirstOrDefault();

        //   byte[] senhaAtual = ModuloGeral.encrypt(model.Password);
        //   var logado = ModuloGeral.comparepwd(usu.Password, senhaAtual);
        //     if (logado)
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, false);
        //            if (!String.IsNullOrEmpty(returnUrl))
        //            {
        //                return Redirect(returnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Admin", "");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Usuário ou senha incorretos.");
        //        }
        //}



        //
        // GET: /Girls/

        public ViewResult Index()
        {
            return View(db.GirlsModels.ToList());
        }

        //
        // GET: /Girls/Details/5

        public ViewResult Details(int id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            return View(girlsmodels);
        }

        //
        // GET: /Girls/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Girls/Create

        [HttpPost]
        public ActionResult Create(GirlsModels girlsmodels)
        {
            if (ModelState.IsValid)
            {
                girlsmodels.situacao = "P";
                db.GirlsModels.Add(girlsmodels);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(girlsmodels);
        }
        
        //
        // GET: /Girls/Edit/5
 
        public ActionResult Edit(int id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            return View(girlsmodels);
        }

        //
        // POST: /Girls/Edit/5

        [HttpPost]
        public ActionResult Edit(GirlsModels girlsmodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(girlsmodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(girlsmodels);
        }

        //
        // GET: /Girls/Delete/5
 
        public ActionResult Delete(int id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            return View(girlsmodels);
        }

        //
        // POST: /Girls/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            db.GirlsModels.Remove(girlsmodels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }
  

        [HttpPost]
        public ActionResult Register(GirlsModels model)
        {
            if (model != null) {
                db.GirlsModels.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }
            return RedirectToAction("Index");
        }

        public ActionResult LogOn()
        {
            return View();
        }

    }
}