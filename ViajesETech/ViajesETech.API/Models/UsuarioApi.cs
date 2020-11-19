using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViajesETech.API.Models
{
    public class UsuarioApi
    {
        public int Id { get; set; }
        public int CI { get; set; }
        [MaxLength(100, ErrorMessage = "El campo Address debe contener solo 100 caracteres")]
        public string Address { get; set; }
        [MaxLength(12, ErrorMessage = "El número solo puede contener 12 caracteres.")]
        public string Phone { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El Name solo puede contener 50 caracteres.")]
        public string Name { get; set; }
        [Required]        
        [MaxLength(20, ErrorMessage = "El userName solo puede contener 20 caracteres.")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El Email solo puede contener 50 caracteres.")]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdUser { get; set; }

    }
}