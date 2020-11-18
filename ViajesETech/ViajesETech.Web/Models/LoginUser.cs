using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViajesETech.Web.Models
{
    public class LoginUser
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}