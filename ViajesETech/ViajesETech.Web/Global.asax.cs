using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ViajesETech.Web
{
    using Data;
    using Dominio.Helpers;
    using Dominio.Data;
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UserMaster();
        }

        private void UserMaster()
        {
            DbContext db = new DbContext();
            if (db.Users.Where(u => u.UserName == "LnietoViajero").Count() == 0)
            {
                db.Users.Add(new User { Email = "luis_m_nieto@hotmail.com", Name = "Luis Miguel Nieto", Password = Encriptador.Cifrar("123456"), Rol = false, UserName = "LnietoViajero" });
                db.Viajeros.Add(new Viajeros { Address = "caracas", CI = 23682466, Phone = "04126091371", User = db.Users.SingleOrDefault(u => u.UserName == "LnietoViajero") });
                db.Destinos.Add(new Destinos { Name = "Caracas" });
                db.Destinos.Add(new Destinos { Name = "Maracay" });
                db.Users.Add(new User { Email = "gonzalez.luis.m.nieto@gmail.com", Name = "Luis Miguel Nieto", Password = Encriptador.Cifrar("123456"), Rol = true, UserName = "Lnieto" });
                db.SaveChanges();
            }
        }
    }
}
