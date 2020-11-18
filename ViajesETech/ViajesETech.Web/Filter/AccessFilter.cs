﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViajesETech.Web.Models;

namespace ViajesETech.Web.Filter
{
    public class AccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Login/Index");
                return;
            }
            var user = (UserLoger)HttpContext.Current.Session["Usuario"];
            var controller = filterContext.RouteData.Values["controller"];
            var action = filterContext.RouteData.Values["action"];
            bool permiso = true;
           
            if (user.Rol)
            {                            
                    permiso = false;                   
            }
            else
            {
                if ((controller.ToString() == "Viajes" && action.ToString() == "Index") )
                {
                }
            }
            if (permiso)
            {
                filterContext.HttpContext.Response.Redirect("~/Inicio/Index");
                return;
            }
            // base.OnActionExecuting(filterContext);
        }
    }
}