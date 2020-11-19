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
    public class DestinosController : ApiController
    {
        ApiContext db = new ApiContext();
        // GET: api/Destinos
        public Result Get()
        {
            return new Result { Data= db.Destinos.ToList(), Status = (int)HttpStatusCode.OK };
        }

        // GET: api/Destinos/5
        public Result Get(int id)
        {
            var d = db.Destinos.Find(id);
            if(d==null)
                return new Result { Message ="El Destino no existe." , Status = (int)HttpStatusCode.NotFound };

            return new Result { Data =d , Status = (int)HttpStatusCode.OK };
        }

        // POST: api/Destinos
        public Result Post([FromBody]Destinos destino)
        {
            if (ModelState.IsValid)
            {
                if(db.Destinos.Where(d=>d.Name==destino.Name).Count()>0)
                    return new Result { Message = "El Destino ya Existe.", Status = (int)HttpStatusCode.Ambiguous };
                db.Destinos.Add(new Destinos { Name = destino.Name });
                db.SaveChanges();
                return new Result {Message="Destino Creado",  Status = (int)HttpStatusCode.Created };
            }
            string rpta = string.Empty;
            var query = (from state in ModelState.Values
                         from error in state.Errors
                         select error.ErrorMessage).ToList();           
            query.ForEach(x => rpta +=  x.ToString() + "\n");                
            return new Result { Message = "El Destino No esta completo." + rpta , Status = (int)HttpStatusCode.BadRequest };
        }

        // PUT: api/Destinos/5
        public Result Put( [FromBody]Destinos value)
        {
            if (ModelState.IsValid)
            {
                if (db.Destinos.Where(d => d.Name == value.Name && d.Id != value.Id).Count() > 0)
                    return new Result { Message = "El Destino ya Existe.", Status = (int)HttpStatusCode.Ambiguous }; 
                db.Entry(value).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new Result {Message="Destino Editado", Status = (int)HttpStatusCode.OK };
            }
            string rpta = string.Empty;
            var query = (from state in ModelState.Values
                         from error in state.Errors
                         select error.ErrorMessage).ToList();
            query.ForEach(x => rpta += x.ToString() + "\n");
            return new Result { Message = "El Destino No esta completo." + rpta, Status = (int)HttpStatusCode.BadRequest };

        }

        // DELETE: api/Destinos/5
        public Result Delete(int id)
        {
            var d = db.Destinos.Find(id);
            if (d == null)
                return new Result { Message = "El Destino no existe.", Status = (int)HttpStatusCode.NotFound };
            db.Destinos.Remove(d);
            db.SaveChanges();
            return new Result { Message = "El Destino Fue eliminado.", Status = (int)HttpStatusCode.OK };
        }
    }
}
