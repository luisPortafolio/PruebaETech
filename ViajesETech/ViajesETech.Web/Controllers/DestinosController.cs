using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViajesETech.Dominio.Data;
using ViajesETech.Web.Data;

namespace ViajesETech.Web.Controllers
{
    public class DestinosController : Controller
    {
        private DbContext db = new DbContext();

        // GET: Destinos
        public ActionResult Index()
        {
            return View( db.Destinos.ToList());
        }

        // GET: Destinos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = await db.Destinos.FindAsync(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            return View(destinos);
        }

        // GET: Destinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Destinos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                db.Destinos.Add(destinos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(destinos);
        }

        // GET: Destinos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = await db.Destinos.FindAsync(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            return View(destinos);
        }

        // POST: Destinos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(destinos).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(destinos);
        }

        // GET: Destinos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = await db.Destinos.FindAsync(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            return View(destinos);
        }

        // POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Destinos destinos = await db.Destinos.FindAsync(id);
            db.Destinos.Remove(destinos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
