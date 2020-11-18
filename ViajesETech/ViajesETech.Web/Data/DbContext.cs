using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesETech.Web.Data
{
    using Dominio.Data;

    public class DbContext : ViajesETechContext 
    {
        public DbContext()  : base()
        {

        }
    }
}