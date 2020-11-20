using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViajesETech.API.Data;
using ViajesETech.API.Models;
using ViajesETech.Dominio.Data;
using ViajesETech.Dominio.Helpers;

namespace ViajesETech.API.Controllers
{
    public class ViajeroController : ApiController
    {
        ApiContext db = new ApiContext();
        // GET: api/Viajero
        public Result Get()
        {
            return new Result { Data = db.Viajeros.Select(v=> new UsuarioApi
            {
             Id = v.Id,
                UserName = v.User.UserName,
                Address = v.Address,
                CI = v.CI,
                Email = v.User.Email,
                IdUser = v.User.Id,
                Name = v.User.Name,
                Phone = v.Phone
            } ).ToList(), Status= (int)HttpStatusCode.OK };
        }

        // GET: api/Viajero/5
        public Result Get(int id)
        {
            var v = db.Viajeros.Find(id);
            if (v == null)
                return new Result { Message = "El Viajero no existe.", Status = (int)HttpStatusCode.NotFound };
            return new Result { Data = db.Viajeros.Where(vi=> v.Id==id).Select(vi => new UsuarioApi
            {
                Id = vi.Id,
                UserName = vi.User.UserName,
                Address = vi.Address,
                CI = vi.CI,
                Email = vi.User.Email,
                IdUser = vi.User.Id,
                Name = vi.User.Name,
                Phone = vi.Phone         
            }), Status = (int)HttpStatusCode.OK };
        }

        // POST: api/Viajero
        public Result Post([FromBody]UsuarioApi value)
        {
            if((db.Viajeros.Where(v=> v.CI==value.CI).Count()>0) || (db.Users.Where(v => v.UserName == value.UserName || v.Email == value.Email).Count() > 0) )
            {
               return new Result { Message = "El usuario ya Existe", Status = (int)HttpStatusCode.BadRequest };
            }
            try
            {
                db.Users.Add(new User
                {
                    Email = value.Email,
                    Name = value.Name,
                    Password = Encriptador.Cifrar(value.Password),
                    Rol = false,
                    UserName = value.UserName
                });
                db.SaveChanges();
                db.Viajeros.Add(new Viajeros
                {
                    Address = value.Address,
                    Phone = value.Phone,
                    CI = value.CI,
                    User = db.Users.Where(u => u.UserName == value.UserName).First()
                });
                db.SaveChanges();
                return new Result { Message = "Creado", Status = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new Result { Message = "Ocurrio un error", Status = (int)HttpStatusCode.InternalServerError };
            }
        }

        // PUT: api/Viajero/5
        public Result Put( [FromBody]UsuarioApi value)
        {
            if (ModelState.IsValid)
            {
                if (db.Viajeros.Where(v => v.User.Name == value.Name && v.Id != value.Id).Count() > 0)
                    return new Result { Message = "El Viajero ya Existe.", Status = (int)HttpStatusCode.Ambiguous };
                try
                {
                    var viajeroEditar = db.Viajeros.Find(value.Id);
                    viajeroEditar.Phone = value.Phone;
                    viajeroEditar.CI = value.CI;
                    viajeroEditar.Address = value.Address;
                    viajeroEditar.User.Name = value.Name;
                    viajeroEditar.User.UserName = value.UserName;
                    viajeroEditar.User.Email = value.Email;
                    db.SaveChanges();
                    return new Result { Message = "Viajero Editado", Status = (int)HttpStatusCode.OK };
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
            return new Result { Message = "El Destino No esta completo." + rpta, Status = (int)HttpStatusCode.BadRequest };
        }

        // DELETE: api/Viajero/5
        public Result Delete(int id)
        {
            var v = db.Viajeros.Find(id);
            if (v == null)
                return new Result { Message = "El Viajero no existe.", Status = (int)HttpStatusCode.NotFound };
            db.Viajeros.Remove(v);
            db.SaveChanges();
            return new Result { Message = "El Viajero Fue eliminado.", Status = (int)HttpStatusCode.OK };

        }
    }
}
