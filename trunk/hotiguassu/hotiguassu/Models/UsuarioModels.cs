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
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string Password { get; set; }

        [Display(Name = "Lembrar senha")]
        public Boolean RememberMe { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
