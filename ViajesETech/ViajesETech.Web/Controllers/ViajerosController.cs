using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViajesETech.Dominio.Data;
using ViajesETech.Web.Data;
using ViajesETech.Web.Filter;
using ViajesETech.Web.Models;

namespace ViajesETech.Web.Controllers
{
    [AccessFilter]
    public class ViajerosController : Controller
    {
        private DbContext db = new DbContext();

        // GET: Viajeros
        public ActionResult Index()
        {
            return View(db.Viajeros.ToList());
        }

        // GET: Viajeros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viajeros viajeros = db.Viajeros.Find(id);
            if (viajeros == null)
            {
                return HttpNotFound();
            }
            return View(viajeros);
        }

      

       /// <summary>
       /// Solicita el id del usuario para poder buscar al viajero.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Edit()
        {
            var user = Session["Usuario"] as UserLoger;
            Viajeros viajeros = db.Viajeros.Where(v=> v.User.Id==user.Id).First();
            if (viajeros == null)
            {
                return HttpNotFound();
            }
            return View(viajeros);
        }

       /// <summary>
       /// Edita el perfil del viajero
       /// </summary>
       /// <param name="viajeros"></param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Viajeros viajeros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viajeros).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Viajes",null);
            }
            return View(viajeros);
        }                     
     

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
