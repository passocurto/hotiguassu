using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotiguassu.Areas.Admin.Controllers
{
    public class AutenticacaoController : Controller 
    {
        public ActionResult Index()
        {
            return View("LogOn");
        }

    }
}
