using System.ComponentModel.DataAnnotations;

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
        public DataType DtNacimento { get; set; }

        [Display(Name = "Biografia")]
        public string Biografia { get; set; }

        [Display(Name = "Telefones")]
        public string Telefones { get; set; }

        [Display(Name = "Altura")]
        public char Altura { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Cabelos")]
        public string Cabelos { get; set; }

        [Display(Name = "Quadris")]
        public string Quadris { get; set; }

        [Display(Name = "Busto")]
        public string Busto { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Login")]
        public string login { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Por favor entre com um endereço de e-mail valido !")]
        [Required(ErrorMessage = "O {0} é requerido.")]
        [StringLength(100, ErrorMessage = "O {0} deve conter no máximo 100 caracteres.")]
        [Display(Name = "E-mail")]
        public string Senha { get; set; }

    }
}