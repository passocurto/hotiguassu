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
            ViewBag.GarotasContrario = from u in db.GirlsModels orderby db.GirlsModels descending select u;
            


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

        public ActionResult AboutGirls(int id)
        {
            GirlsModels girlsmodels = db.GirlsModels.Find(id);
            if (girlsmodels != null)
                return View("AboutGirls", girlsmodels);
            return View("Index");
        }

        public ActionResult About()
        {
            return View("About");
        }

    }
}
