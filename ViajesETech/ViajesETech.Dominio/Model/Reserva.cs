using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViajesETech.Dominio.Model
{
    public class Reserva
    {
        [Required]
        public int PlaceRequired { get; set; }
        public decimal Price { get; set; }
        public int IdViaje { get; set; }
       
    }
}
