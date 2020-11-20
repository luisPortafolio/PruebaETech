using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ViajesETech.Dominio.Data;
using Newtonsoft.Json;

namespace ViajesETech.API.Models
{
    public class ReservaViaje
    {
        public int Id { get; set; }
        [Required]        
        public  int IdViajes { get; set; }        
        [Required]        
        public  int IdViajeros { get; set; }
        [Required]
        public int Place { get; set; }
        
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}