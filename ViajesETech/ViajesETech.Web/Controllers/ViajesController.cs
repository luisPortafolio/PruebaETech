using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViajesETech.Dominio.Data;
using ViajesETech.Dominio.Model;
using ViajesETech.Web.Data;
using ViajesETech.Web.Filter;

namespace ViajesETech.Web.Controllers
{
    [AccessFilter]
    public class ViajesController : Controller
    {
        private DbContext db = new DbContext();

        #region Acciones Controlador
        // GET: Viajes
        public ActionResult Index()
        {
            return View(db.Viajes.ToList());
        }

        // GET: Viajes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viajes viajes = db.Viajes.Find(id);
            if (viajes == null)
            {
                return HttpNotFound();
            }
            return View(viajes);
        }

        // GET: Viajes/Create
        public ActionResult Create()
        {
            LLenarCombosDestinos();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viajes"></param>
        /// <returns>Devuelve a la lista de todos los viajes si se pudo crear exitosamente.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Viajes viajes)
        {
            if (ModelState.IsValid && viajes.DestinoFi!=viajes.DestinoOrig)
            {
                viajes.DestinosFin = db.Destinos.Find(viajes.DestinoFi);
                viajes.DestinosOrigen = db.Destinos.Find(viajes.DestinoOrig);
                viajes.PlaceDisponibles = viajes.Place;
                db.Viajes.Add(viajes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LLenarCombosDestinos();
            return View(viajes);
        }

        // GET: Viajes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LLenarCombosDestinos();
            Viajes viajes = db.Viajes.Find(id);
            if (viajes == null)
            {
                return HttpNotFound();
            }
            viajes.DestinoFi = viajes.DestinosFin.Id;
            viajes.DestinoOrig = viajes.DestinosOrigen.Id;
            return View(viajes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Viajes viajes)
        {
            if (ModelState.IsValid && viajes.DestinoFi != viajes.DestinoOrig)
            {
                 var modificado =db.Viajes.Find(viajes.Id);
                if(modificado.PlaceDisponibles<=viajes.Place)
                    return View(viajes);
                modificado.Place = viajes.Place;
                modificado.Price = viajes.Price;
                modificado.Code = viajes.Code;
                modificado.DestinosFin = db.Destinos.Find(viajes.DestinoFi);
                modificado.DestinosOrigen = db.Destinos.Find(viajes.DestinoOrig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LLenarCombosDestinos();
            return View(viajes);
        }

        // GET: Viajes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Viajes viajes = db.Viajes.Find(id);
            if (viajes == null)
            {
                return HttpNotFound();
            }
            return View(viajes);
        }

        // POST: Viajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Viajes viajes = db.Viajes.Find(id);
            db.Viajes.Remove(viajes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
        #region Method internos

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Llena los combo box para facilitar para facilitar la creación y edición de los destinos.
        /// </summary>
        private void LLenarCombosDestinos()
        {
            List<SelectListItem> lista = (from d in db.Destinos
                                          select new SelectListItem
                                          {
                                              Text = d.Name,
                                              Value = d.Id.ToString()
                                          }).ToList();
            lista.Insert(0, new SelectListItem { Value = string.Empty, Text = "---SELECCIONE---" });
            ViewBag.listaDestinos = lista;
        }


        public ActionResult Selected(int id)
        {

            return View();
        }
        #endregion

    }
}
