using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace hotiguassu.Models
{
    [Table("tbGerUsuario")]
    public class UsuarioModels
    {

        [Key]
        public int idUsuario { get; set; }

        [Required]
        [Display(Name = "Longin")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
