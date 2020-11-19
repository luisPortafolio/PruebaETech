using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViajesETech.Dominio.Data;
using ViajesETech.Dominio.Model;
using ViajesETech.Web.Data;
using ViajesETech.Web.Filter;
using ViajesETech.Web.Models;

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
            if (ModelState.IsValid && viajes.DestinoFi != viajes.DestinoOrig)
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
                var modificado = db.Viajes.Find(viajes.Id);
                if (modificado.PlaceDisponibles <= viajes.Place)
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

        /// <summary>
        /// Selecciona el Viaje para hacer la reserva del mismo.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Selected(int id)
        {                                
            return View(db.Viajes.Find(id));
        }
        /// <summary>
        /// Selecciona el Viaje para hacer la reserva del mismo.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditViajeViajero(int id)
        {
            var viajeViajero = db.ViajesViajeros.Find(id);
            ViewBag.plazas = viajeViajero.Place;

            return View("Selected",db.Viajes.Find(id));
        }

        /// <summary>
        /// Reserva el viaje con el precio y la cantidad y hace el descuento de los disponibles del total disponible 
        /// </summary>
        /// <param name="reserva"></param>
        /// <returns></returns>
        public ActionResult Reserva(Reserva reserva)
        {
            var user = Session["Usuario"] as UserLoger;
            db.ViajesViajeros.Add(new ViajesViajeros
            {
                Place = reserva.PlaceRequired,
                Price = reserva.Price,
                Viajes = db.Viajes.Find(reserva.IdViaje),
                Viajeros = db.Viajeros.Where(v => v.User.Id == user.Id).First()
            });
            db.Viajes.Find(reserva.IdViaje).PlaceDisponibles = db.Viajes.Find(reserva.IdViaje).PlaceDisponibles - reserva.PlaceRequired;
            db.SaveChanges();
            return RedirectToAction("ViajesViajero");
        }
        /// <summary>
        /// Lista de los Viajes del usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult ViajesViajero()
        {
            var user = Session["Usuario"] as UserLoger;
            return View(db.ViajesViajeros.Where(v => v.Viajeros.User.Id == user.Id).ToList());
        }

        /// <summary>
        /// Detalle del viaje seleccionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViajesViajeroDetail(int id)
        {
            return View(db.ViajesViajeros.Find(id));
        }

        /// <summary>
        /// reporte en PDF de los viajes del usuario
        /// </summary>
        /// <returns></returns>
        public FileResult ViajesViajeroReport()
        {
            var user = Session["Usuario"] as UserLoger;
            byte[] path = FileCreation(user);
            return File(path, "application/pdf");
        }
        /// <summary>
        /// reporte en PDF de los viajes de todos los usuarios
        /// </summary>
        /// <returns></returns>
        public FileResult ViajesViajeroReportTotal()
        {
           
            byte[] path = FileCreation();
            return File(path, "application/pdf");
        }
        /// <summary>
        /// Se encarga de generar la data para el archivo.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private byte[] FileCreation(UserLoger user=null)
        {
            var list = user == null ? db.ViajesViajeros.ToList() : db.ViajesViajeros.Where(v => v.Viajeros.User.Id == user.Id).ToList();
            Document doc = new Document();
            byte[] path;
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                //Creamos el parrafo del titulo 
                Paragraph title = new Paragraph("Mis Viajes");
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph(" "));
                //cantidad de columnas que tendra la tabla 
                PdfPTable table = new PdfPTable(6);
                //variable que me dara las dimensiones de la tabla
                float[] values = new float[6] { 10, 40, 40, 15, 25, 30 };
                table.SetWidths(values);
                //Creando las celdas
                PdfPCell cell = new PdfPCell(new Phrase("Id"));
                cell.BackgroundColor = new BaseColor(130, 130, 130);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                PdfPCell cell2 = new PdfPCell(new Phrase("Origen"));
                cell2.BackgroundColor = new BaseColor(130, 130, 130);
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase("Destino"));
                cell3.BackgroundColor = new BaseColor(130, 130, 130);
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell3);
                PdfPCell cell4 = new PdfPCell(new Phrase("Place"));
                cell4.BackgroundColor = new BaseColor(130, 130, 130);
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell4);
                PdfPCell cell5 = new PdfPCell(new Phrase("Price"));
                cell5.BackgroundColor = new BaseColor(130, 130, 130);
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase("Total"));
                cell6.BackgroundColor = new BaseColor(130, 130, 130);
                cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell6);
                foreach (var item in list)
                {
                    table.AddCell(item.Id.ToString());
                    table.AddCell(item.Viajes.DestinosOrigen.Name);
                    table.AddCell(item.Viajes.DestinosFin.Name);
                    table.AddCell(item.Place.ToString());
                    table.AddCell(item.Price.ToString());
                    table.AddCell((item.Price * item.Place).ToString());
                }
                doc.Add(table);
                doc.AddTitle("Reporte Mis Viajes");
                doc.AddLanguage("es");
                doc.AddAuthor("Luis Nieto");
                doc.Close();
                path = ms.ToArray();
            }

            return path;
        }

        public ActionResult ViajesViajerosTotal()
        {
            return View("ViajesViajeros", db.ViajesViajeros.ToList());
        }

        

        #endregion

    }
}
