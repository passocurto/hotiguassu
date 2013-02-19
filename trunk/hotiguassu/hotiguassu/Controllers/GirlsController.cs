using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hotiguassu.Models;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace hotiguassu.Controllers
{
    public class GirlsController : Controller
    {
        private hotiguassuContext db = new hotiguassuContext();


        [HttpPost]
        public ActionResult LogOn(GirlsModels models)
        {
            var q = from u in db.GirlsModels
                    where u.login == models.login
                    select u;

            var usu = q.FirstOrDefault();

            var logado = false;
            if (usu.Senha == models.Senha)
                logado = true;

            if (logado)
            {
                FormsAuthentication.SetAuthCookie(models.login, false);
                GravaCookieGirl(Convert.ToString(usu.idGirl));
                return View("index");
            }
            else
            {
                ModelState.AddModelError("", "usuário ou senha incorretos.");
            }
            return View("LogOn");
        }

        private void GravaCookieGirl(string IdGirl)
        {
            HttpCookie cookieidGirl;

            if (Request.Cookies["idGirl"] == null)
            {
                cookieidGirl = new HttpCookie("idGirl");
            }
            else
            {
                cookieidGirl = Request.Cookies["idGirl"];
            }

            cookieidGirl.Value = IdGirl;

            Response.Cookies.Add(cookieidGirl);

            cookieidGirl.Expires = DateTime.Now.AddDays(1);
        }


        //
        // GET: /Girls/

        public ViewResult Index()
        {
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie("login", false);

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

            var model = new GirlsModels();
            model.TipodeCabelo = new[]
                {
                    // TODO: those values could come from a database for example
                    new SelectListItem { Value = "Loira", Text = "Loira" },
                    new SelectListItem { Value = "Morena", Text = "Morena" },
                    new SelectListItem { Value = "Ruiva", Text = "Ruiva" },
                };

            return View(model);
        }

        //
        // POST: /Girls/Create

        [HttpPost]
        public ActionResult Create(GirlsModels girlsmodels)
        {
            if (girlsmodels != null)
            {
                string dtNascimento = Request.Form["DtNacimento"];
                girlsmodels.situacao = "P";
                gravaTelefone(girlsmodels);
                if (dtNascimento != "")
                {
                    girlsmodels.DtNacimento = DateTime.Parse(dtNascimento);
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                FormsAuthentication.SetAuthCookie(girlsmodels.login, false);
                db.GirlsModels.Add(girlsmodels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(girlsmodels);
        }

        private void gravaTelefone(GirlsModels girlsmodels)
        {
            string Telefone = "";
            foreach (string key in HttpContext.Request.Form.AllKeys.Where(k => k.StartsWith("opcaoTelefone")))
            {
                if (Request.Form[key] != "")
                {
                    Telefone = Telefone + Request.Form[key] + ";";
                }
            }
            Telefone = Telefone.Replace("-", "");
            Telefone = Telefone.Replace(")", "");
            Telefone = Telefone.Replace("(", "");
            Telefone = Regex.Replace(Telefone, " ", "");
            Telefone = Telefone.Remove(Telefone.Length - 1, 1);
            girlsmodels.Telefones = Telefone;
            Telefone = null;
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
            if (girlsmodels != null)
            {
                db.Entry(girlsmodels).State = EntityState.Modified;
                gravaTelefone(girlsmodels);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        //
        // GET: /Girls/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            return View(girlsmodels);
        }

        //
        // POST: /Girls/Delete/5
        
        [HttpPost]
        public ActionResult Delete(string id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(int.Parse(id));
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
            if (model != null)
            {
                db.GirlsModels.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult LogOn()
        {
            var model = new GirlsModels();
            return View(model);
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

    }
}