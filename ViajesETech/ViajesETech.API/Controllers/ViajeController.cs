using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViajesETech.API.Data;
using ViajesETech.API.Models;
using ViajesETech.Dominio.Data;

namespace ViajesETech.API.Controllers
{
    public class ViajeController : ApiController
    {
        ApiContext db = new ApiContext();
        // GET: api/Viaje
        public Result Get()
        {
            return new Result
            {
                Data = db.Viajes.Select(v => new ViajeApi
                {
                    Code = v.Code,
                    DestinoFi = v.DestinosFin.Id,
                    Id = v.Id,
                    Place = v.Place,
                    DestinoOrig = v.DestinosOrigen.Id,
                    PlaceDisponibles = v.PlaceDisponibles,
                    Price = v.Price
                }).ToList(),
                Status = (int)HttpStatusCode.OK
            };
        }

        // GET: api/Viaje/5
        public Result Get(int id)
        {
            if (db.Viajes.Find(id) == null)
                return new Result { Message = "No Existe", Status = (int)HttpStatusCode.NotFound };
            return new Result
            {
                Data = db.Viajes.Where(v => v.Id == id).Select(v => new ViajeApi
                {
                    Code = v.Code,
                    DestinoFi = v.DestinosFin.Id,
                    Id = v.Id,
                    Place = v.Place,
                    DestinoOrig = v.DestinosOrigen.Id,
                    PlaceDisponibles = v.PlaceDisponibles,
                    Price = v.Price
                }).First(),
                Status = (int)HttpStatusCode.OK
            };
        }

        // POST: api/Viaje
        public Result Post([FromBody]ViajeApi value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Viajes.Add(new Viajes
                    {
                        Code = value.Code,
                        Place = value.Place,
                        PlaceDisponibles = value.PlaceDisponibles,
                        Price = value.Price,
                        DestinosFin = db.Destinos.Find(value.DestinoFi),
                        DestinosOrigen = db.Destinos.Find(value.DestinoOrig)
                    });
                    return new Result { Message = "Creado", Status = (int)HttpStatusCode.OK };
                }
                catch (Exception ex)
                {
                    return new Result { Message = "Ocurrio un error", Status = (int)HttpStatusCode.InternalServerError };
                }
            }
            string rpta = string.Empty;
            var query = (from state in ModelState.Values
                         from error in state.Errors
                         select error.ErrorMessage).ToList();
            query.ForEach(x => rpta += x.ToString() + "\n");
            return new Result { Message = rpta, Status = (int)HttpStatusCode.BadRequest };
        }

        // PUT: api/Viaje
        public Result Put([FromBody]ViajeApi value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var v = db.Viajes.Find(value.Id);
                    v.Code = value.Code;
                    v.Place = value.Place;
                    v.PlaceDisponibles = value.PlaceDisponibles;
                    v.Price = value.Price;
                    v.DestinosFin = db.Destinos.Find(value.DestinoFi);
                    v.DestinosOrigen = db.Destinos.Find(value.DestinoOrig);
                    db.SaveChanges();
                    return new Result { Message = "Editado", Status = (int)HttpStatusCode.OK };
                }
                catch (Exception ex)
                {
                    return new Result { Message = "Ocurrio un error", Status = (int)HttpStatusCode.InternalServerError };
                }
            }
            string rpta = string.Empty;
            var query = (from state in ModelState.Values
                         from error in state.Errors
                         select error.ErrorMessage).ToList();
            query.ForEach(x => rpta += x.ToString() + "\n");
            return new Result { Message = rpta, Status = (int)HttpStatusCode.BadRequest };
        }

        // DELETE: api/Viaje/5
        public Result Delete(int id)
        {
            if (db.Viajes.Find(id) == null)
                return new Result { Message = "No Existe", Status = (int)HttpStatusCode.NotFound };
            db.Viajes.Remove(db.Viajes.Find(id));
            db.SaveChanges();
            return new Result { Message = "Eliminado Exitosamente.", Status = (int)HttpStatusCode.OK };
        }
    }
}
