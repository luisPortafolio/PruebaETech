using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViajesETech.Web.Controllers
{
    using Data;
    public class ViajesController : Controller
    {
        private DbContext bd;

        public ViajesController()
        {
            bd = new DbContext();
        }
        // GET: Viajes
        public ActionResult Index()
        {    
            return View(bd.Viajes.ToList());
        }
    }
}