using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViajesETech.API.Models
{
    public class ViajeApi
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El campo solo puede tener 50 caracteres")]          
        public string Code { get; set; }
        public int Place { get; set; }
        public int PlaceDisponibles { get; set; }        
        [Required]
        public decimal Price { get; set; }         
        [Required]
        [Display(Name = "Origen")]
        public int DestinoOrig { get; set; }          
        [Required]
        [Display(Name = "Destino")]
        public int DestinoFi { get; set; }        
    }
}