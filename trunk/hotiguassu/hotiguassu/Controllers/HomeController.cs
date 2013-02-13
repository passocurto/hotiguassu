using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotiguassu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Contato()
        {
            return View("Contato");
        }

        public ActionResult Garotas()
        {
            return View("Garotas");
        }

        public ActionResult AboutGirls()
        {
            return View("AboutGirls");
        }

        public ActionResult About()
        {
            return View("About");
        }

    }
}
