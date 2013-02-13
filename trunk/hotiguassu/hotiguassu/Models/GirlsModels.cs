using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hotiguassu.Models
{
    [Table("tbGerGirl")]
    public class GirlsModels
    {

        [Key]
        public int idGirl { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Data de nascimento")]
        public string DtNacimento { get; set; }

        [Display(Name = "Biografia")]
        public string Biografia { get; set; }

        [Display(Name = "Telefones")]
        public string Telefones { get; set; }

        [Display(Name = "Altura")]
        public string Altura { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Cabelos")]
        public string Cabelos { get; set; }

        [Display(Name = "Quadris")]
        public string Quadris { get; set; }

        [Display(Name = "Busto")]
        public string Busto { get; set; }




    }
}