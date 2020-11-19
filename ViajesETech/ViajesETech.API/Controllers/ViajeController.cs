using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViajesETech.API.Data;
using ViajesETech.API.Models;

namespace ViajesETech.API.Controllers
{
    public class ViajeController : ApiController
    {
        ApiContext db = new ApiContext();
        // GET: api/Viaje
        public Result Get()
        {
            return new Result { Data = db.Viajes.Select(v => new ViajeApi
            {
                Code = v.Code,
                DestinoFi = v.DestinosFin.Id,
                Id = v.Id,
                Place = v.Place,
                DestinoOrig = v.DestinosOrigen.Id,
                PlaceDisponibles = v.PlaceDisponibles,
                Price = v.Price }).ToList(), Status = (int)HttpStatusCode.OK
            };
        }

        // GET: api/Viaje/5
        public Result Get(int id)
        {
            return new Result
            {
                Data = db.Viajes.Where(v=> v.Id==id).Select(v => new ViajeApi
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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Viaje/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Viaje/5
        public void Delete(int id)
        {
        }
    }
}
