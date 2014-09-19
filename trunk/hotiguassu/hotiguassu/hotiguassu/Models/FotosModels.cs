using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace hotiguassu.Models
{
    [Table("tbGerFotos")]
    public class FotosModels
    {

        [Key, Column(Order = 0)]
        public int idFoto { get; set; }


        //[ForeignKey("idGirl"), Column(Order = 1)]
        public int idGirl { get; set; }
        
        
        
        [Required]
        [Display(Name = "Nome")]
        public string nmFoto { get; set; }

        [Required]
        [Display(Name = "Extensão")]
        public string nmExtensao { get; set; }

        [Required]
        [Display(Name = "Host")]
        public string nmHost { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public char tipoFoto { get; set; }

        [Required]
        [Display(Name = "Data Upload")]
        public DateTime dtUpload { get; set; }
        
        [Required]
        [Display(Name = "Situacao")]
        public char nmSituacao { get; set; }

        public virtual GirlsModels Girl { get; set; }
    }

}
