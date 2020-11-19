using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViajesETech.API.Data;
using ViajesETech.Dominio.Data;
using ViajesETech.Dominio.Helpers;
using ViajesETech.Dominio.Model;

namespace ViajesETech.API.Controllers
{
    [AllowAnonymous]
    public class RegisterController : ApiController
    {
        private ApiContext db;

        public RegisterController()
        {
            db = new ApiContext();
        }

        [HttpPost]
        public string Registrar(UserRegister userRegister)
        {
            string rpta = string.Empty;
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                rpta = "<ul class = 'list-group'>";
                query.ForEach(x => rpta += "<li class='list-group-item'><p class='text-danger'>" + x.ToString() + "</p></li>");
                rpta += "</ul>";
                return rpta;
            }
            if (db.Users.Where(u => u.UserName == userRegister.UserName).Count() != 0)
            {
                return "User Name Ya en uso, intente con otro.";
            }
            if (db.Users.Where(u => u.Email == userRegister.Email).Count() != 0)
            {
                return "Email Ya en uso, intente con otro.";
            }
            try
            {
                db.Users.Add(new User
                {
                    Email = userRegister.Email,
                    Name = userRegister.Name,
                    Password = Encriptador.Cifrar(userRegister.Password),
                    Rol = false,
                    UserName = userRegister.UserName
                });
                db.Viajeros.Add(new Viajeros
                {
                    CI = userRegister.CI,
                    Address = userRegister.Address,
                    Phone = userRegister.Phone,
                    User = db.Users.SingleOrDefault(u => u.UserName == userRegister.UserName)
                });
                rpta = (db.SaveChanges() > 0) ? "1" : "Intetne de nuevo.";
            }
            catch (Exception ex)
            {
                rpta = "Ocurrio un error intente nuevamente.";
            }
            return rpta;
        }
    }
}
