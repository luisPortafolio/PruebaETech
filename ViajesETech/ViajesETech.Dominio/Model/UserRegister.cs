using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViajesETech.Dominio.Data;

namespace ViajesETech.Dominio.Model
{
    [NotMapped]
    public class UserRegister:User
    {
       
        [Compare("Password")]
        public string RepitePassword { get; set; }
        
    }
    [NotMapped]
    public class ViajeroRegister: Viajeros
    {
        public int IdUsuario { get; set; }
    }
}
