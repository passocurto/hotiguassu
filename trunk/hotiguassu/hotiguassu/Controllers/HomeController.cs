using System.Linq;
using System.Web.Mvc;
using hotiguassu.Models;


namespace hotiguassu.Controllers
{
    public class HomeController : Controller
    {

        private hotiguassuContext db = new hotiguassuContext();

        public ActionResult Index()
        {
            ViewBag.listaGarotas = from u in db.GirlsModels select u;

            return View();
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
