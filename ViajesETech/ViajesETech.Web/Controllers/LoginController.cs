using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViajesETech.Web.Controllers
{
    using Data;
    using Dominio.Helpers;
    using Models;
    using ViajesETech.Dominio.Data;

    public class LoginController : Controller
    {
        #region Atributos
        private DbContext db;
        #endregion

        #region Constructor
        public LoginController()
        {
            db = new DbContext();
        }
        #endregion
        // GET: Login
        #region Methods
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Valida y permite el login en el sistema.
        /// </summary>
        /// <param name="login">objeto que contiene el userName y el password para la validación.</param>
        /// <returns>retorna 1 si existe, de lo contrario devuelve el error.</returns>
        [HttpPost]
        public string Login(LoginUser login)
        {
            string error = string.Empty;
            if (!ModelState.IsValid)
            {
                return "Ingrese los datos requeridos.";
            }
            var passwordCifrado = Encriptador.Cifrar(login.Password);
            if (db.Users.Where(u => u.UserName == login.User && u.Password == passwordCifrado).ToList().Count != 1)
            {
                return "Usuario o contraseña incorrectos.";
            }
            try
            {
                Session["Usuario"] = db.Users.
                                        Where(u => u.UserName == login.User && u.Password == passwordCifrado).
                                        ToList().Select(u =>
                                        new UserLoger
                                        {
                                            Id = u.Id,
                                            Email = u.Email,
                                            Name = u.Name,
                                            Rol = u.Rol,
                                            UserName = u.UserName
                                        }).First();
                error = "1";
            }
            catch (Exception ex)
            {
                error = "Ocurrio un error intente nuevamente.";
            }
            return error;
        }
        /// <summary>
        ///         Pemite el registro de un nuevo usuario.
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns>retorna 1 si se puedo crear, de lo contrario el error respectivo.</returns>
        [HttpPost]
        public string Register(UserRegister userRegister)
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
                rpta = (db.SaveChanges() > 0) ? "1" : "Intetne de nuevo.";
            }
            catch (Exception ex)
            {
                rpta = "Ocurrio un error intente nuevamente.";
            }
            return rpta;
        }

        #endregion

    }
}