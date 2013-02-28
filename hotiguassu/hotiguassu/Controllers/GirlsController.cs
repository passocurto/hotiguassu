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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections;


namespace hotiguassu.Controllers
{
    public class GirlsController : Controller
    {
        private hotiguassuContext db = new hotiguassuContext();
        private static byte[] bIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };
        private const string cryptoKey = "Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=";

        [HttpPost]
        public ActionResult LogOn(GirlsModels models)
        {
            var q = from u in db.GirlsModels
                    where u.login == models.login
                    select u;

            var usu = q.FirstOrDefault();

            string senhaCriptografada = Encoding.UTF8.GetString(usu.Senha);
            string senhaDescriptografada = Decrypt(senhaCriptografada);

            if (senhaDescriptografada.Equals(Request.Form["Senha"]))
            {
                FormsAuthentication.SetAuthCookie(models.login, false);
                GravaSessionGirl(Convert.ToString(usu.idGirl));
                return View("index");
            }
            else
            {
                ModelState.AddModelError("", "usuário ou senha incorretos.");
            }
            return View("LogOn");
        }

 

        private void GravaSessionGirl(string IdGirl)
        {
            Session["idGirl"] = IdGirl;
            Session.Timeout = 30;
        }


        //
        // GET: /Girls/

        public ActionResult Index()
        {
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie("login", false);
            if (Session["idGirl"] != null)
            {
                return View(db.GirlsModels.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
                girlsmodels.Senha = Encrypt(Request.Form["Senha"]);
                db.Configuration.ValidateOnSaveEnabled = false;
                FormsAuthentication.SetAuthCookie(girlsmodels.login, false);
                db.GirlsModels.Add(girlsmodels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(girlsmodels);
        }

        

        public static byte[] Encrypt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bKey = Convert.FromBase64String(cryptoKey);
                byte[] bText = new UTF8Encoding().GetBytes(text);

                Rijndael rijndael = new RijndaelManaged();

                rijndael.KeySize = 256;

                MemoryStream mStream = new MemoryStream();
                CryptoStream encryptor = new CryptoStream(
                    mStream,
                    rijndael.CreateEncryptor(bKey, bIV),
                    CryptoStreamMode.Write);

                encryptor.Write(bText, 0, bText.Length);
                encryptor.FlushFinalBlock();
                var str = Convert.ToBase64String(mStream.ToArray());
                byte[] retorno = System.Text.Encoding.UTF8.GetBytes(str);
                return retorno;
            }
            else
            {
                return null;
            }
        }

        public static string Decrypt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bKey = Convert.FromBase64String(cryptoKey);
                byte[] bText = Convert.FromBase64String(text);

                Rijndael rijndael = new RijndaelManaged();

                rijndael.KeySize = 256;

                MemoryStream mStream = new MemoryStream();

                CryptoStream decryptor = new CryptoStream(
                    mStream,
                    rijndael.CreateDecryptor(bKey, bIV),
                    CryptoStreamMode.Write);

                decryptor.Write(bText, 0, bText.Length);
                decryptor.FlushFinalBlock();
                UTF8Encoding utf8 = new UTF8Encoding();
                return utf8.GetString(mStream.ToArray());
            }
            else
            {
                return null;
            }
        }


        private void gravaTelefone(GirlsModels girlsmodels)
        {
            string Telefone = "";
            string str = Request.Form["opcaoTelefone1"];
            string[] phones = str.Split(',');
            foreach (var telefone in phones)
            {
                if (telefone != "")
                {
                    Telefone = Telefone + telefone + ";";
                }
            }
            if (Telefone.Length > 0)
            {
                Telefone = Telefone.Replace("-", "");
                Telefone = Telefone.Replace(")", "");
                Telefone = Telefone.Replace("(", "");
                Telefone = Regex.Replace(Telefone, " ", "");
                Telefone = Telefone.Remove(Telefone.Length - 1, 1);
                girlsmodels.Telefones = Telefone;
                Telefone = null;
            }

        }

        //
        // GET: /Girls/Edit/

        public ActionResult Edit()
        {
            string id = Session["idGirl"] != null ? (string)Session["idGirl"] : null;
            if (id != null)
            {
                GirlsModels girlsmodels = db.GirlsModels.Find(int.Parse(id));
                return View(girlsmodels);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //
        // POST: /Girls/Edit/

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
        // GET: /Girls/Fotos/
        [HttpGet]
        public ActionResult Fotos()
        {
            string id = Session["idGirl"] != null ? (string)Session["idGirl"] : null;
            if (id != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult DeleteFotos()
        {
            if (Session["idGirl"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteFotos(string fileName)
        {

            if (Session["idGirl"] != null)
            {
                var caminho = "~/Fotos/" + Session["idGirl"];
                string DeleteThis = fileName;
                string[] arquivos = Directory.GetFiles(Server.MapPath(caminho));
                foreach (string aquivo in arquivos)
                {
                    if (aquivo.ToUpper().Contains(DeleteThis.ToUpper()))
                    {
                        System.IO.File.Delete(Server.MapPath(caminho) + "/" + fileName);
                        //Deleta Minhatura
                        System.IO.File.Delete(Server.MapPath(caminho) + "/" + "Minhatura" + "/" + fileName);
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        //
        // GET: /Girls/Delete/
        [HttpGet]
        public ActionResult Delete()
        {
            string id = Session["idGirl"] != null ? (string)Session["idGirl"] : null;
            if (id != null)
            {
                GirlsModels girlsmodels = db.GirlsModels.Find(int.Parse(id));
                return View(girlsmodels);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //
        // POST: /Girls/Delete/

        [HttpPost]
        public ActionResult Delete(int idGirl)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(idGirl);
            db.Entry(girlsmodels).State = EntityState.Modified;
            girlsmodels.situacao = "C";
            db.Configuration.ValidateOnSaveEnabled = false;
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
            Session.Remove("idGirl");
            return RedirectToAction("Index", "Home");
        }



    }
}