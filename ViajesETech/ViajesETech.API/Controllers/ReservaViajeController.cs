﻿using System;
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
    public class ReservaViajeController : ApiController
    {
        ApiContext db = new ApiContext();
        // GET: api/ReservaViaje
        public Result Get([FromUri]int idViajero)
        {
            if (db.ViajesViajeros.Where(x => x.Viajeros.Id == idViajero).Count() <= 0)
                return new Result { Message = "No Posee Viajes", Status = (int)HttpStatusCode.NotFound };
            return new Result
            {
                Data = db.ViajesViajeros.Where(x => x.Viajeros.Id == idViajero).
                Select(x => new ReservaViaje
                {
                    Place = x.Place,
                    Id = x.Id,
                    Price = x.Price,
                    IdViajeros = x.Viajeros.Id,
                    IdViajes = x.Viajes.Id,
                    Total = (x.Price * x.Place)
                }).ToList(),
                Status = (int)HttpStatusCode.OK
            };
        }

        // GET: api/ReservaViaje/5

        public Result Get([FromUri(Name = "id")]int id, [FromUri(Name = "idViajero")]int idViajero)
        {
            if (id <= 0 || idViajero <= 0)
                return new Result { Message = "No puede ser nulo.", Status = (int)HttpStatusCode.BadRequest };
            if (db.ViajesViajeros.Find(id) == null)
                return new Result { Message = "No Existe", Status = (int)HttpStatusCode.NotFound };
            return new Result
            {
                Data = db.ViajesViajeros.Where(x => x.Id == id).
                Select(x => new ReservaViaje
                {
                    Place = x.Place,
                    Id = x.Id,
                    Price = x.Price,
                    IdViajeros = x.Viajeros.Id,
                    IdViajes = x.Viajes.Id,
                    Total = (x.Price * x.Place)
                }).ToList().First(),
                Status = (int)HttpStatusCode.OK
            };
        }

        // POST: api/ReservaViaje
        public Result Post([FromBody]ReservaViaje value)
        {
            if (value == null)
                return new Result { Message = "No puede ser nulo.", Status = (int)HttpStatusCode.BadRequest };
            var viaje = db.Viajes.Find(value.IdViajes);
            if (viaje.PlaceDisponibles < 0 && viaje.PlaceDisponibles < value.Place && value.Place <= 0)
                return new Result { Message = "No se puede reservar, No hay place disponibles.", Status = (int)HttpStatusCode.NotFound };
            if (ModelState.IsValid)
            {
                try
                {
                    db.ViajesViajeros.Add(new ViajesViajeros
                    {
                        Place = value.Place,
                        Price = db.Viajes.Find(value.IdViajes).Price,
                        Viajeros = db.Viajeros.Find(value.IdViajeros),
                        Viajes = db.Viajes.Find(value.IdViajes)
                    });
                    viaje.PlaceDisponibles-= value.Place;
                    db.SaveChanges();
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

        // PUT: api/ReservaViaje/5
        public Result Put( [FromBody]ReservaViaje value)
        {
            if(value==null)
                return new Result { Message = "No puede ser nulo.", Status = (int)HttpStatusCode.BadRequest };
            var viaje = db.Viajes.Find(value.IdViajes);
            if (viaje.PlaceDisponibles < 0 && viaje.PlaceDisponibles < value.Place && value.Place <= 0)
                return new Result { Message = "No se puede reservar, No hay place disponibles.", Status = (int)HttpStatusCode.NotFound };

            if (ModelState.IsValid)
            {
                try
                {
                    var v = db.ViajesViajeros.Find(value.Id);
                    if (v.Viajes.Id != value.IdViajes)                              //Validamos si el cambio fue el viaje.
                    {
                        var viajeAnterior = db.Viajes.Find(v.Viajes.Id);           //buscamos el viaje anterior 
                        viajeAnterior.PlaceDisponibles += v.Place;                 //colocamos las plazas nuevamente disponibles
                        viaje.PlaceDisponibles -= value.Place;                     //restamos las plazas que requerimos
                        db.Entry(viajeAnterior).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(viaje).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        v.Viajes = db.Viajes.Find(value.IdViajes);             //actualizamos el viaje
                        v.Place = value.Place;                                 //actualizamos las plazas
                        v.Price = viaje.Price;                                 //actualizamos el precio
                        v.Viajeros = db.Viajeros.Find(value.IdViajeros);       //actualizamos el Viajero
                        db.Entry(v).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else if (v.Place > value.Place)                                         //validamos si las plazas son menores 
                    {
                        viaje.PlaceDisponibles += (v.Place - value.Place);                  //actualizamos las plazas disponibles sumando las que ya no se requieren
                        db.Entry(viaje).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        v.Place = value.Place;                                              //actualizamos las plazas
                        v.Price = viaje.Price;                                              //actualizamos el precio
                        v.Viajeros = db.Viajeros.Find(value.IdViajeros);                    //actualizamos el Viajero
                        db.Entry(v).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else if (v.Place < value.Place)                                        //validamos si las plazas son mayores a las anteriores
                    {
                        viaje.PlaceDisponibles -= (value.Place - v.Place);                  //actualizamos los disponibles restando los que adquirimos
                        db.Entry(viaje).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        v.Place = value.Place;                                              //actualizamos las plazas
                        v.Price = viaje.Price;                                              //actualizamos el precio
                        v.Viajeros = db.Viajeros.Find(value.IdViajeros);                    //actualizamos el Viajero   
                        db.Entry(v).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
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

        // DELETE: api/ReservaViaje/5
        public Result Delete(int id)
        {
            if (db.ViajesViajeros.Find(id) == null)
                return new Result { Message = "No Existe", Status = (int)HttpStatusCode.NotFound };
            db.ViajesViajeros.Remove(db.ViajesViajeros.Find(id));
            db.SaveChanges();
            return new Result { Message = "Eliminado Exitosamente.", Status = (int)HttpStatusCode.OK };
        }
    }
}
