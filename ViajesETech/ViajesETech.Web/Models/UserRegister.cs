using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesETech.Web.Models
{
    using Dominio.Data;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class UserRegister : User
    {                         
        [Compare("Password")]
        public string RepitePassword { get; set; }
    }
}